namespace ReferenceService.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using ReferenceService.WebApi.Models;
    using ReferenceService.WebApi.Validators;

    /// <summary>
    /// Тесты валидации
    /// </summary>
    [TestFixture]
    public class ValidationTests
    {
        private static readonly CreateReferenceRequestValidator CreateReferenceRequestValidator =
            new CreateReferenceRequestValidator();

        /// <summary>
        /// Тест валидации входящего запроса для создания справки (валидный)
        /// </summary>
        [Test]
        public void ValidationTests_CreateReferenceValidTest()
        {
            var result = CreateReferenceRequestValidator.Validate(new CreateReferenceRequest
            {
                Code = "2",
                RegistrationChannel = "BO",
                Customer = new Customer
                {
                    CUID = "TEST",
                    FIO = "TEST",
                    PassportSerNum = "TEST"
                },
                User = new User
                {
                    ID = "TEST",
                    FIO = "TEST",
                    LocalDateTime = DateTime.Now,
                    Workstation = new Workstation
                    {
                        IP = "127.0.0.1",
                        Name = "TEST"
                    }
                },
                Params = new Dictionary<string, string>()
            });

            Assert.IsTrue(result.IsValid);
        }

        /// <summary>
        /// Тест валидации входящего запроса для создания справки (не валидный)
        /// </summary>
        [Test]
        public void ValidationTests_CreateReferenceInvalidTest()
        {
            var result = CreateReferenceRequestValidator.Validate(new CreateReferenceRequest
            {
                Code = "6"
            });

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Where(p => p.ErrorCode == "1").Any());
            Assert.IsTrue(result.Errors.Where(p => p.ErrorCode == "2").Any());
        }
    }
}
