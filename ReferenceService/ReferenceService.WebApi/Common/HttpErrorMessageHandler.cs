namespace ReferenceService.WebApi.Common
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    using NLog;
    using ReferenceService.Common.Log;
    
    /// <summary>
    /// Обработка HTTP-ошибок
    /// </summary>
    public class HttpErrorMessageHandler : DelegatingHandler
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Обрабатывает HTTP-ошибки
        /// </summary>
        /// <param name="request">HTTP-запрос</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>HTTP-ответ</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestBody = null;
            object requestObject = null;
            Exception parseException = null;

            var id = request.GetCorrelationId().ToString("N");

            try
            {
                requestBody = await request.Content.ReadAsStringAsync();
                requestObject = JsonConvert.DeserializeObject(requestBody);
            }
            catch(Exception ex)
            {
                parseException = ex;
            }

            var requestDetails = new HttpLogDetails(HttpLogDetails.ConvertHeaders(request.Headers))
            {
                Body = requestObject ?? requestBody,
                URL = request.RequestUri.ToString(),
                Method = request.Method.ToString()
            };
            Log(LogLevel.Info, "REQUEST", id, requestDetails);

            var message = await base.SendAsync(request, cancellationToken);

            if (message.StatusCode == HttpStatusCode.NotFound ||
                message.StatusCode == HttpStatusCode.MethodNotAllowed ||
                message.StatusCode == HttpStatusCode.UnsupportedMediaType)
            {
                message.Content = null;
                return message;
            }

            if (parseException != null)
            {
                Log(LogLevel.Error, "RESPONSE_400", id, new
                {
                    body = requestBody,
                    exception = parseException
                });

                return request.CreateResponse(HttpStatusCode.BadRequest, new AdditionalInformation
                {
                    Errors = new[]
                    {
                        new Error
                        {
                            Code = "1000",
                            InternalMessage = parseException.Message
                        }
                    }
                });
            }

            object responseObject = null;
            var logLevel = LogLevel.Info;

            if (message.StatusCode != HttpStatusCode.OK &&
                message.StatusCode != HttpStatusCode.Created)
            {
                logLevel = LogLevel.Error;

                string exceptionMessage = string.Empty;

                var exceptionMessageContent = message.Content as ObjectContent<Exception>;
                
                if (exceptionMessageContent != null && exceptionMessageContent.Value != null)
                {
                    responseObject = exceptionMessageContent.Value;
                    exceptionMessage = ((Exception)responseObject).Message;

                    message.Content = new ObjectContent<AdditionalInformation>(new AdditionalInformation
                    {
                        Errors = new[]
                        {
                            new Error
                            {
                                Code = "1000",
                                InternalMessage = exceptionMessage
                            }
                        }
                    }, new JsonMediaTypeFormatter());
                }
            }

            if (responseObject == null)
            {
                responseObject = message.Content != null ? await message.Content.ReadAsAsync<object>() : null;
            }

            var responseDetails = new HttpLogDetails(HttpLogDetails.ConvertHeaders(message.Headers))
            {
                StatusCode = message.StatusCode,
                Body = responseObject
            };
            Log(logLevel, "RESPONSE_" + (int)message.StatusCode, id, responseDetails);

            return message;
        }

        /// <summary>
        /// Лог
        /// </summary>
        /// <param name="level">Уровень логирования</param>
        /// <param name="message">Сообщение</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="details">Дополнительная информация</param>
        private void Log(LogLevel level, string message, string id, object details = null)
        {
            LogHelper.Log(_logger, level, message, id, details);
        }
    }
}
