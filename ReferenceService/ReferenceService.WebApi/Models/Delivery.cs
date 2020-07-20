namespace ReferenceService.WebApi.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Информация о доставке Справки
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// Способ доставки Справки
        /// </summary>
        [JsonProperty("method")]
        public string Method { get; set; }

        /// <summary>
        /// Email, на который отправляется справка
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Номер БО или АП, в котором печатается справка.
        /// </summary>
        [JsonProperty("boNumber")]
        public string BONumber { get; set; }

        /// <summary>
        /// Наименование ТТ АП, на котором печатается справка
        /// </summary>
        [JsonProperty("posName")]
        public string PosName { get; set; }

        /// <summary>
        /// Адрес ТТ АП, на котором печатается справка
        /// </summary>
        [JsonProperty("posAddr")]
        public string PosAddr { get; set; }

    }
}