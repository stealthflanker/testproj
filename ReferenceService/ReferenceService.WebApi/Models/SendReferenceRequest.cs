namespace ReferenceService.WebApi.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Запрос для отправки справки
    /// </summary>
    public class SendReferenceRequest
    {
        /// <summary>
        /// Раздел с информацией о доставке Справки
        /// </summary>
        [JsonProperty("delivery")]
        public Delivery Delivery { get; set; }
    }
}
