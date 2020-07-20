namespace ReferenceService.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using ReferenceService.WebApi.Common;
    using ReferenceService.WebApi.Controllers;
    using ReferenceService.WebApi.Entities;
    using ReferenceService.WebApi.Interfaces;
    using ReferenceService.WebApi.Models;

    /// <summary>
    /// Тесты контроллера ReferenceController
    /// </summary>
    [TestFixture]
    public class ReferenceControllerTests
    {
        private class Repository : IReferenceRepository
        {
            public readonly Dictionary<string, Reference> References = 
                new Dictionary<string, Reference>();

            public Task<Reference> Create(Reference reference, CancellationToken cancellationToken)
            {
                References.Add(reference.Number, reference);
                return Task.FromResult(reference);
            }

            public Task<IEnumerable<Reference>> Find(Expression<Func<Reference, bool>> predicate)
            {
                return Task.FromResult(References.Values.Where(predicate.Compile()));
            }

            public Task<Reference> Get(string refNumber, CancellationToken cancellationToken)
            {
                Reference reference = null;
                References.TryGetValue(refNumber, out reference);
                return Task.FromResult(reference);
            }

            public Task Update(Reference reference, CancellationToken cancellationToken)
            {
                References[reference.Number] = reference;
                return Task.FromResult(0);
            }
        }

        private class PrintServerBridge : IPrintServerBridge
        {
            public Task Store(Reference reference, IDictionary<string, string> refParams, string id, CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
        }

        private class BadPrintServerBridge : IPrintServerBridge
        {
            public Task Store(Reference reference, IDictionary<string, string> refParams, string id, CancellationToken cancellationToken)
            {
                throw new ReferenceServiceException(100, "PDF_ERROR");
            }
        }

        private class EventHub : IEventHub
        {
            public Task Send(Reference reference, string id, CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
        }

        private class BadEventHub : IEventHub
        {
            public Task Send(Reference reference, string id, CancellationToken cancellationToken)
            {
                throw new ReferenceServiceException(101, "SEND_ERROR");
            }
        }

        private CreateReferenceRequest GetCreateReferenceRequest()
        {
            return new CreateReferenceRequest
            {
                Code = "1",
                RegistrationChannel = "BO",
                Customer = new Customer
                {
                    CUID = "CUID",
                    FIO = "FIO",
                    PassportSerNum = "PassportSerNum"
                },
                User = new User
                {
                    ID = "ID",
                    FIO = "FIO",
                    LocalDateTime = DateTime.Now,
                    Workstation = new Workstation
                    {
                        IP = "IP",
                        Name = "Name"
                    }
                },
                Params = new Dictionary<string, string>()
            };
        }

        /// <summary>
        /// Тест создания справочника
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_CreateTest()
        {
            var repository = new Repository();

            var _controller = new ReferenceController(repository, new PrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Create(GetCreateReferenceRequest())).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

            CreateReferenceResponse result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Number));
        }

        /// <summary>
        /// Тест создания справочника
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_Create_BadPSTest()
        {
            var repository = new Repository();

            var _controller = new ReferenceController(repository, new BadPrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Create(GetCreateReferenceRequest())).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreEqual(repository.References["1"].Status, Constants.ReferenceStatusPdfError);

            AdditionalInformation result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Errors);
            Assert.IsTrue(result.Errors.Where(p => p.Code == "100").Any());
        }

        /// <summary>
        /// Тест отправки справочника
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_SendTest()
        {
            var request = new SendReferenceRequest
            {
                Delivery = new Delivery
                {
                    Method = Constants.DeliveryMethodBO
                }
            };

            var repository = new Repository();
            repository.References.Add("0", new Reference { Number = "0", Status = Constants.ReferenceStatusPdfOk });

            var _controller = new ReferenceController(repository, new PrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Send("0", request)).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(repository.References["0"].Status, Constants.ReferenceStatusDelivered);
        }

        /// <summary>
        /// Тест отправки справочника (not found)
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_Send_NullReferenceTest()
        {
            var request = new SendReferenceRequest
            {
                Delivery = new Delivery
                {
                    Method = Constants.DeliveryMethodBO
                }
            };

            var _controller = new ReferenceController(new Repository(), new PrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Send("0", request)).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);

            AdditionalInformation result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.NotNull(result.Errors);
            Assert.IsTrue(result.Errors.Where(e => e.Code == "150").Any());
        }

        /// <summary>
        /// Тест отправки справочника (pdf error)
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_Send_PdfErrorTest()
        {
            var request = new SendReferenceRequest
            {
                Delivery = new Delivery
                {
                    Method = Constants.DeliveryMethodBO
                }
            };

            var repository = new Repository();
            repository.References.Add("0", new Reference { Number = "0", Status = Constants.ReferenceStatusPdfError });

            var _controller = new ReferenceController(repository, new PrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Send("0", request)).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, (HttpStatusCode) 422);

            AdditionalInformation result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.NotNull(result.Errors);
            Assert.IsTrue(result.Errors.Where(e => e.Code == "151").Any());
        }

        /// <summary>
        /// Тест отправки справочника (pdf error)
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_Send_BadEHTest()
        {
            var request = new SendReferenceRequest
            {
                Delivery = new Delivery
                {
                    Method = Constants.DeliveryMethodEmail,
                    Email = "TEST"
                }
            };

            var repository = new Repository();
            repository.References.Add("0", new Reference { Number = "0", Status = Constants.ReferenceStatusPdfOk });

            var _controller = new ReferenceController(repository, new PrintServerBridge(), new BadEventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.Send("0", request)).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);

            AdditionalInformation result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.NotNull(result.Errors);
            Assert.IsTrue(result.Errors.Where(e => e.Code == "101").Any());
        }

        /// <summary>
        /// Тест истории справочника
        /// </summary>
        /// <returns>Task</returns>
        [Test]
        public async Task ReferenceController_HistoryTest()
        {
            var repository = new Repository();
            repository.References.Add("1", new Reference { Status = Constants.ReferenceStatusDelivered, CustomerCUID = "1" });
            repository.References.Add("2", new Reference { Status = Constants.ReferenceStatusDelivered, CustomerCUID = "1" });
            repository.References.Add("3", new Reference { Status = Constants.ReferenceStatusDelivered, CustomerCUID = "1" });
            repository.References.Add("4", new Reference { Status = Constants.ReferenceStatusDelivered, CustomerCUID = "2" });
            repository.References.Add("5", new Reference { Status = Constants.ReferenceStatusPdfOk, CustomerCUID = "1" });

            var _controller = new ReferenceController(repository, new PrintServerBridge(), new EventHub())
            {
                Configuration = Helper.CreateHttpConfiguration(),
                Request = new HttpRequestMessage()
            };

            var response = await (await _controller.History("1")).ExecuteAsync(new CancellationToken());

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            IEnumerable<ReferenceHistoryItem> result = null;
            response.TryGetContentValue(out result);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.AreEqual(result.Count(), 3);
        }
    }
}
