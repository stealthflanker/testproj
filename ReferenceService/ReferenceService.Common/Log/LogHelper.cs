namespace ReferenceService.Common.Log
{
    using NLog;

    /// <summary>
    /// Вспомогательный класс для логирования
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Вспомогательный метод для логирования
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="level">Уровень логирования</param>
        /// <param name="message">Сообщение</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="details">Дополнительная информация</param>
        public static void Log(ILogger logger, LogLevel level, string message, string id, object details = null)
        {
            var logEvent = new LogEventInfo
            {
                Level = level,
                Message = message,
            };
            logEvent.Properties.Add("id", id);
            logEvent.Properties.Add("details", details);
            logger.Log(logEvent);
        }
    }
}
