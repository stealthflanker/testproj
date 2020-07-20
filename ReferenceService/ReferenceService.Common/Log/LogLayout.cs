namespace ReferenceService.Common.Log
{
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using NLog;
    using NLog.Config;
    using NLog.Layouts;
    
    /// <summary>
    /// Формат логов
    /// </summary>
    [Layout("LogLayout")]
    public class LogLayout : Layout
    {
        /// <summary>
        /// ID процесса
        /// </summary>
        [RequiredParameter]
        public Layout PID { get; set; }

        private class LogItem
        {
            public LogItem()
            {
                Timestamp = DateTime.Now;
            }

            [JsonProperty("@timestamp")]
            public DateTime Timestamp { get; private set; }

            [JsonProperty("level")]
            public string Level { get; set; }

            [JsonProperty("pid")]
            public string PID { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
            public string ID { get; set; }

            [JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)]
            public object Details { get; set; }
        }

        /// <summary>
        /// <see cref="Layout.GetFormattedMessage(LogEventInfo)"/>
        /// </summary>
        /// <param name="logEvent">LogEventInfo</param>
        /// <returns>String</returns>
        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            var logItem = new LogItem
            {
                Level = logEvent.Level.Name.ToUpper(),
                PID = PID.Render(logEvent),
                Message = logEvent.Message
            };

            if (logEvent.Properties.ContainsKey("details"))
            {
                logItem.Details = logEvent.Properties["details"];
            }

            if (logEvent.Properties.ContainsKey("id"))
            {
                var id = logEvent.Properties["id"] as string;
                if (!string.IsNullOrEmpty(id))
                {
                    logItem.ID = id;
                }
            }

            return JsonConvert.SerializeObject(logItem, Formatting.None);
        }
    }
}
