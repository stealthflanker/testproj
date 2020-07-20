namespace ReferenceService.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Сущность информации о клиенте
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// CUID Клиента
        /// </summary>
        [Required, JsonRequired, JsonProperty("cuid")]
        public string CUID { get; set; }

        /// <summary>
        /// ФИО Клиента
        /// </summary>
        [Required, JsonRequired, JsonProperty("fio")]
        public string FIO { get; set; }

        /// <summary>
        /// Серия и номер паспорта клиента
        /// </summary>
        [Required, JsonRequired, JsonProperty("passportSerNum")]
        public string PassportSerNum { get; set; }
    }
}
