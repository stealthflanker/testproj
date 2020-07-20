namespace ReferenceService.WebApi.Common
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using Controllers;

    /// <summary>
    /// Поиск контроллера по роутингу
    /// </summary>
    public class ControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _configuration;

        /// <summary>
        /// Создаёт экземпляр класса <see cref="ControllerSelector"/>
        /// </summary>
        /// <param name="configuration">HTTP-конфигурация</param>
        public ControllerSelector(HttpConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Возвращает дексриптор контроллера по роутингу
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Дескриптор контроллера</returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                return base.SelectController(request);
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode != HttpStatusCode.NotFound &&
                    ex.Response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                    ex.Response.StatusCode != HttpStatusCode.UnsupportedMediaType)
                {
                    throw;
                }

                return new HttpControllerDescriptor(_configuration, Constants.ErrorControllerName, typeof(ErrorController));
            }
        }
    }
}
