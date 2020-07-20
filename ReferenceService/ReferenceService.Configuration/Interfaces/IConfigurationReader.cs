namespace ReferenceService.Configuration.Interfaces
{
    /// <summary>
    /// Интерфейс чтения конфигурации
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Возвращает значение параметра конфигурации
        /// </summary>
        /// <param name="key">Ключ параметра конфигурации</param>
        /// <param name="default">Значение по умолчанию</param>
        /// <returns>Значаение параметра конфигурации</returns>
        string Get(string key, string @default = null);
    }
}
