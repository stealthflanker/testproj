namespace ReferenceService.WebApi.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Контроллер ошибок
    /// </summary>
    public class ErrorController : ApiController
    {
        /// <summary>
        /// Обработчик HTTP-ошибки 404
        /// </summary>
        /// <returns>HTTP-ответ</returns>
        [HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions, AcceptVerbs("PATCH")]
        public HttpResponseMessage Handle404()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
    }
}
