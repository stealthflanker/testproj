namespace ReferenceService.WebApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Сущность информации о пользователе, сформировавшего справку
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "1"), JsonRequired, JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [Required, JsonRequired, JsonProperty("fio")]
        public string FIO { get; set; }

        /// <summary>
        /// Локальная дата и время пользователя сформировавшего справку
        /// </summary>
        [Required, JsonRequired, JsonProperty("localDateTime")]
        public DateTime? LocalDateTime { get; set; }

        /// <summary>
        /// Информация о рабочей станции
        /// </summary>
        [Required, JsonRequired, JsonProperty("workstation")]
        public Workstation Workstation { get; set; }
    }
}
