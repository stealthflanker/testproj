namespace ReferenceService.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Web.Http.Description;
    using Common;
    using FluentValidation;
    using Interfaces;
    using Mappers;
    using Models;
    using Newtonsoft.Json;
    using Validators;

    /// <summary>
    /// Контроллер справок
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("formreference/v1")]
    public class ReferenceController : ApiController
    {
        private const string Salt = "62E52C7E99D641249F6CA97AB7D22105";

        private readonly IReferenceRepository _repository;
        private readonly IPrintServerBridge _printServerBridge;
        private readonly IEventHub _eventHub;

        private static readonly CreateReferenceRequestValidator CreateReferenceRequestValidator = 
            new CreateReferenceRequestValidator();

        private static readonly SendReferenceRequestValidator SendReferenceRequestValidator =
            new SendReferenceRequestValidator();

        /// <summary>
        /// Создаёт экземпляр API-контроллера <see cref="ReferenceController"/>
        /// </summary>
        /// <param name="repository">Репозиторий справок</param>
        /// <param name="printServerBridge">Клиент PrintServer</param>
        /// <param name="eventHub">Клиент EventHub</param>
        public ReferenceController(IReferenceRepository repository, IPrintServerBridge printServerBridge, IEventHub eventHub)
        {
            _repository = repository;
            _printServerBridge = printServerBridge;
            _eventHub = eventHub;
        }

        /// <summary>
        /// Ping test
        /// </summary>
        /// <returns>OK</returns>
        [HttpGet, Route(@"ping", Name = Salt + "Ping")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Ping()
        {
            return Ok();
        }

        /// <summary>
        /// HTTP 500 test
        /// </summary>
        /// <returns>InternalServerError</returns>
        [HttpGet, Route(@"fail", Name = Salt + "Fail")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Fail()
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Exception test
        /// </summary>
        /// <returns>Exception</returns>
        [HttpGet, Route(@"fatal", Name = Salt + "Fatal")]
        [ResponseType(typeof(string))]
        public IHttpActionResult Fatal()
        {
            throw new Exception("Fatal");
        }

        /// <summary>
        /// Создаёт справку
        /// </summary>
        /// <param name="request">Запрос для создания справки</param>
        /// <returns>Результат создания справки</returns>
        [HttpPost, Route(@"references", Name = Salt + "Create")]
        [ResponseType(typeof(CreateReferenceResponse))]
        public async Task<IHttpActionResult> Create([FromBody] CreateReferenceRequest request)
        {
            if (request == null)
            {
                return Error(HttpStatusCode.BadRequest, new[]
                {
                    new Error
                    {
                        Code = "1000",
                        InternalMessage = Resources.Strings.ErrorRequestRequired
                    }
                });
            }

            var errors = Validate(request, CreateReferenceRequestValidator);
            if (errors != null)
            {
                return errors;
            }

            var reference = CreateReferenceMapper.Map(request);

            var cancellationToken = new CancellationToken();

            reference = await _repository.Create(reference, cancellationToken);

            try
            {
                await _printServerBridge.Store(reference, request.Params, Request.GetCorrelationId().ToString("N"), cancellationToken);
            }
            catch(ReferenceServiceException ex)
            {
                reference.Status = Constants.ReferenceStatusPdfError;
                await _repository.Update(reference, cancellationToken);
                if (ex.Code == 102)
                    return Error(HttpStatusCode.InternalServerError, new[]
                    {
                        new Error
                        {
                            Code = ex.Code.ToString(),
                            UserMessage = "На данный момент автоматическое формирование справки невозможно.",
                            InternalMessage = ex.Message                           
                        }
                    });
                else
                    return Error(HttpStatusCode.InternalServerError, ex.ToErrors());
            }

            reference.Status = Constants.ReferenceStatusPdfOk;
            await _repository.Update(reference, cancellationToken);

            return Created(request.Code, new CreateReferenceResponse
            {
                Number = reference.Number
            });
        }

        /// <summary>
        /// Отправляет справку
        /// </summary>
        /// <param name="refNumber">Номер справки</param>
        /// <param name="request">Запрос для отправки справки</param>
        /// <returns>Результат отправки справки</returns>
        [HttpPut, Route(@"references/{refNumber}/send", Name = Salt + "Send")]
        [ResponseType(typeof(AdditionalInformation))]
        public async Task<IHttpActionResult> Send(string refNumber, [FromBody] SendReferenceRequest request)
        {
            if (string.IsNullOrEmpty(refNumber))
                return NotFound();

            if (request == null)
            {
                return Error(HttpStatusCode.BadRequest, new[]
                {
                    new Error
                    {
                        Code = "1000",
                        InternalMessage = Resources.Strings.ErrorRequestRequired
                    }
                });
            }

            var errors = Validate(request, SendReferenceRequestValidator);
            if (errors != null)
            {
                return errors;
            }

            var cancellationToken = new CancellationToken();

            var reference = await _repository.Get(refNumber, cancellationToken);
            if (reference == null)
            {
                return Error(HttpStatusCode.NotFound, new[]
                {
                    new Error
                    {
                        Code = "150",
                        InternalMessage = Resources.Strings.ReferenceServiceError150,
                    }
                });
            }

            if (reference.Status != Constants.ReferenceStatusPdfOk)
            {
                return Error((HttpStatusCode) 422, new[]
                {
                    new Error
                    {
                        Code = "151",
                        InternalMessage = Resources.Strings.ReferenceServiceError151,
                    }
                }, "Unprocessable Entity");
            }

            reference.Status = Constants.ReferenceStatusSended;
            reference.DeliveryMethod = request.Delivery.Method;
            reference.DeliveryEmail = request.Delivery.Email;            

            if (request.Delivery.Method == Constants.DeliveryMethodEmail)
            {
                try
                {
                    await _eventHub.Send(reference, Request.GetCorrelationId().ToString("N"), cancellationToken);
                }
                catch (ReferenceServiceException ex)
                {
                    return Error(HttpStatusCode.InternalServerError, ex.ToErrors());
                }
            }

            reference.Status = Constants.ReferenceStatusDelivered;
            await _repository.Update(reference, cancellationToken);
                        
            return Ok();
        }

        /// <summary>
        /// Предоставляет историю справок
        /// </summary>
        /// <param name="cuid">CUID клиента</param>
        /// <returns>История справок</returns>
        [HttpGet, Route(@"references/delivered", Name = Salt + "History")]
        [ResponseType(typeof(IEnumerable<ReferenceHistoryItem>))]
        public async Task<IHttpActionResult> History([FromUri(Name = "customer")] string cuid = null)
        {
            if(string.IsNullOrEmpty(cuid))
            {
                return Error(HttpStatusCode.BadRequest, new[]
                {
                    new Error
                    {
                        Code = Constants.ErrorCodeRequired,
                        InternalMessage = Resources.Strings.ReferenceServiceError1                        
                    }
                });
            }

            var references = await _repository.Find(r => r.Status == Constants.ReferenceStatusDelivered && r.CustomerCUID == cuid);
            if(!references.Any())
                return Ok();

            return Ok(references.Select(r => ReferenceHistoryMapper.Map(r)));
        }

        /// <summary>
        /// Сформировать ошибку
        /// </summary>
        /// <param name="statusCode">HTTP код</param>
        /// <param name="errors">Коллекция ошибок</param>
        /// <param name="reasonPhrase">Причина</param>
        /// <returns>Сформированная ошибка</returns>
        private IHttpActionResult Error(HttpStatusCode statusCode, IEnumerable<Error> errors, string reasonPhrase = null)
        {
            var response = Request.CreateResponse(statusCode, new AdditionalInformation
            {
                Errors = errors
            });
            response.ReasonPhrase = reasonPhrase;
            return ResponseMessage(response);
        }

        /// <summary>
        /// Проверить корректность запроса
        /// </summary>        
        /// <param name="request">Запрос</param>
        /// <param name="validator">Соответствующий валидатор</param>
        /// <returns>Результат проверки</returns>
        private IHttpActionResult Validate<TRequest>(TRequest request, AbstractValidator<TRequest> validator)
        {
            var result = validator.Validate(request);
            if (result.IsValid)
            {
                return null;
            }

            var error = result.Errors.First();
            var internalMessage = Resources.Strings.ResourceManager.GetString("ReferenceServiceError" + error.ErrorCode);
            var errors = new[] 
            {
                new Error
                {
                    Code = error.ErrorCode,
                    InternalMessage = internalMessage
                }
            };

            return Error(HttpStatusCode.BadRequest, errors);
        }
    }
}
