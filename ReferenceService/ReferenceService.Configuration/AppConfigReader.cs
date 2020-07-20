namespace ReferenceService.Configuration
{
    using System;
    using System.Configuration;
    using Interfaces;

    /// <summary>
    /// Читает значения параметров конфигурации из секции appSettings в файле app.config
    /// </summary>
    internal class AppConfigReader : IConfigurationReader
    {
        /// <summary>
        /// Возвращает значение параметра конфигурации
        /// </summary>
        /// <param name="key">Ключ параметра конфигурации</param>
        /// <param name="default">Значение по умолчанию</param>
        /// <returns>Значаение параметра конфигурации</returns>
        public string Get(string key, string @default = null)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            return ConfigurationManager.AppSettings[key] ?? @default;
        }
    }
}
