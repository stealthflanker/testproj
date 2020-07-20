namespace ReferenceService.WebApi.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Параметры справки для PrintServer
    /// </summary>
    public class ReferenceParams
    {
        /// <summary>
        /// Номер договора
        /// </summary>
        [JsonProperty("deliveryMethod")]
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        [JsonProperty("contract")]
        public string Contract { get; set; }

        /// <summary>
        /// Номер счёта
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Дата начала выписки
        /// </summary>
        [JsonProperty("dateFrom")]
        public string DateFrom { get; set; }

        /// <summary>
        /// Дата окончания выписки
        /// </summary>
        [JsonProperty("dateTo")]
        public string DateTo { get; set; }
    }
}