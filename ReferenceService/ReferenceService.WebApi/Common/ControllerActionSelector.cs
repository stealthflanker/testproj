namespace ReferenceService.WebApi.Common
{
    using System.Net;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    /// <summary>
    /// Поиск метода в контроллере по роутингу
    /// </summary>
    public class ControllerActionSelector : ApiControllerActionSelector
    {
        /// <summary>
        /// Возвращает дексриптор метода контроллера по роутингу
        /// </summary>
        /// <param name="controllerContext">Контекст контроллера</param>
        /// <returns>Дескриптор метода</returns>
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            try
            {
                return base.SelectAction(controllerContext);
            }
            catch (HttpResponseException ex)
            {
                if (ex.Response.StatusCode == HttpStatusCode.NotFound)
                {
                    controllerContext.RouteData.Values["action"] = Constants.ErrorNotFoundHandle;
                }

                return base.SelectAction(controllerContext);
            }
        }
    }
}
