namespace ReferenceService.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    /// <summary>
    /// Дополнительная информация для ответа
    /// </summary>
    public class AdditionalInformation
    {
        /// <summary>
        /// Ошибки
        /// </summary>
        [JsonProperty("errors", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Error> Errors { get; set; }
    }
}
