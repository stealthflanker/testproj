namespace ReferenceService.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Сущность информации о рабочей станции
    /// </summary>
    public class Workstation
    {
        /// <summary>
        /// IP адрес рабочей станции
        /// </summary>
        [Required, JsonRequired, JsonProperty("ip")]
        public string IP { get; set; }

        /// <summary>
        /// Название рабочей станции
        /// </summary>
        [Required, JsonRequired, JsonProperty("name")]
        public string Name { get; set; }
    }
}
