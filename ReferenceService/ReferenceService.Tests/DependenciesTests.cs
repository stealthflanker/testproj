namespace ReferenceService.Tests
{
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Http.Dispatcher;
    using NUnit.Framework;
    using ReferenceService.WebApi.Interfaces;

    /// <summary>
    /// Тесты зависимостей
    /// </summary>
    [TestFixture]
    public class DependenciesTests
    {
        ServicesContainer _services;
        IDependencyResolver _dependencies;

        /// <summary>
        /// Инициализация тестов
        /// </summary>
        [SetUp]
        public void Init()
        {
            var configuration = Helper.CreateHttpConfiguration();
            _services = configuration.Services;
            _dependencies = configuration.DependencyResolver;
        }

        /// <summary>
        /// Тест разрешения зависимостей
        /// </summary>
        [Test]
        public void ResolveDependenciesTest()
        {
            Assert.NotNull(_services.GetService(typeof(IHttpControllerSelector)));
            Assert.NotNull(_services.GetService(typeof(IHttpActionSelector)));

            Assert.NotNull(_dependencies.GetService(typeof(IReferenceRepository)));
        }
    }
}
