namespace ReferenceService.Tests.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using System.Web.Http.Hosting;
    using NUnit.Framework;
    using ReferenceService.WebApi.Controllers;
    
    /// <summary>
    /// Тесты для роутинга
    /// </summary>
    [TestFixture]
    public class RoutingTests
    {
        private class Param
        {
            public Uri Url;
            public HttpMethod Method;
            public Type ControllerType;
            public string ActionName;
        }

        private static readonly List<Param> Params = new List<Param>
        {
            new Param
            {
                Url = new Uri("http://localhost/formreference/v1/references"),
                Method = HttpMethod.Post,
                ControllerType = typeof(ReferenceController),
                ActionName = "Create"
            },
            new Param
            {
                Url = new Uri("http://localhost/formreference/v1/references/test/send"),
                Method = HttpMethod.Put,
                ControllerType = typeof(ReferenceController),
                ActionName = "Send"
            },
            new Param
            {
                Url = new Uri("http://localhost/formreference/v1/references/delivered"),
                Method = HttpMethod.Get,
                ControllerType = typeof(ReferenceController),
                ActionName = "History"
            },
            new Param
            {
                Url = new Uri("http://localhost/formreference/v1"),
                Method = HttpMethod.Post,
                ControllerType = typeof(ErrorController),
                ActionName = "Handle404"
            }
        };

        HttpConfiguration _configuration;

        /// <summary>
        /// Инициализация тестов
        /// </summary>
        [SetUp]
        public void Init()
        {
            _configuration = Helper.CreateHttpConfiguration();
        }

        /// <summary>
        /// Тестирование поиска контроллера и метода
        /// </summary>
        /// <param name="paramIndex">Индекс параметра в Params</param>
        [Test, Sequential]
        public void FindControllerAndActionTest([Values(0, 1, 2, 3)] int paramIndex)
        {
            var param = Params[paramIndex];

            var request = new HttpRequestMessage
            {
                RequestUri = param.Url,
                Method = param.Method
            };

            var routeData = _configuration.Routes.GetRouteData(request);

            request.Properties[HttpPropertyKeys.HttpConfigurationKey] = _configuration;
            request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;

            var controllerSelector = _configuration
                .Services
                .GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;
            
            Assert.NotNull(controllerSelector);

            var controller = controllerSelector.SelectController(request);

            Assert.AreEqual(controller.ControllerType, param.ControllerType);

            var actionSelector = _configuration
                .Services
                .GetService(typeof(IHttpActionSelector)) as IHttpActionSelector;

            Assert.NotNull(actionSelector);

            var controllerContext = new HttpControllerContext(_configuration, routeData, request);
            controllerContext.ControllerDescriptor = controller;

            var action = actionSelector.SelectAction(controllerContext);

            Assert.AreEqual(action.ActionName, param.ActionName);
        }
    }
}
