namespace ReferenceService.WebApi.Models
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Элемент истории справок
    /// </summary>
    public class ReferenceHistoryItem
    {
        /// <summary>
        /// Номер справки
        /// </summary>
        [JsonProperty("refNumber")]
        public string Number { get; set; }

        /// <summary>
        /// Код справки
        /// </summary>
        [JsonProperty("refCode")]
        public string Code { get; set; }

        /// <summary>
        /// Название справки
        /// </summary>
        [JsonProperty("refName")]
        public string Name { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        [JsonProperty("createDateTime")]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// Канал оформления справки
        /// </summary>
        [JsonProperty("refRegChannel")]
        public string RegistrationChannel { get; set; }

        /// <summary>
        /// Информация о пользователе
        /// </summary>
        [JsonProperty("user")]
        public User User { get; set; }

        /// <summary>
        /// Информация о клиенте
        /// </summary>
        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Информация о доставке
        /// </summary>
        [JsonProperty("delivery")]
        public Delivery Delivery { get; set; }
    }
}