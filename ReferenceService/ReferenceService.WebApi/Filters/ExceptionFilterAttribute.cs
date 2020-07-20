namespace ReferenceService.WebApi.Filters
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Filters;
    using NLog;
    using ReferenceService.Common.Log;
    
    /// <summary>
    /// Фильтр исключений
    /// </summary>
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Вызывается при запуске исключения
        /// </summary>
        /// <param name="actionExecutedContext">Контекст метода контроллера</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception as HttpResponseException;
            if(exception != null)
            {
                actionExecutedContext.Response = exception.Response;
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, actionExecutedContext.Exception);
            }
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
