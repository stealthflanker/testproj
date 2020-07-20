namespace ReferenceService.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;   

    /// <summary>
    /// Сущность ошибки
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Конструктор для <see cref="Error"/>
        /// </summary>
        public Error()
        {
            UserMessage = Resources.Strings.ReferenceServiceErrorUserMessage;
        }

        /// <summary>
        /// Текст ошибки для пользователя
        /// </summary>
        [Required, JsonRequired, JsonProperty("userMessage")]
        public string UserMessage { get; set; }

        /// <summary>
        /// Технический текст ошибки
        /// </summary>
        [Required, JsonRequired, JsonProperty("internalMessage")]
        public string InternalMessage { get; set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        [Required, JsonRequired, JsonProperty("code")]
        public string Code { get; set; }
    }
}
