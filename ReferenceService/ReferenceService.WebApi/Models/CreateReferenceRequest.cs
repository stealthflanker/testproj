namespace ReferenceService.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    /// <summary>
    /// Запрос для создания справки
    /// </summary>
    public class CreateReferenceRequest
    {
        /// <summary>
        /// Код справки
        /// </summary>
        [JsonProperty("refCode")]
        public string Code { get; set; }

        /// <summary>
        /// Канал оформления справки
        /// </summary>
        [JsonProperty("refRegChannel")]
        public string RegistrationChannel { get; set; }

        /// <summary>
        /// Информация о пользователе, сформировавшем справку
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// Информация о клиенте
        /// </summary>
        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Параметры справки для PrintServer
        /// </summary>
        [JsonProperty("refParams")]
        public IDictionary<string, string> Params { get; set; }
    }
}
