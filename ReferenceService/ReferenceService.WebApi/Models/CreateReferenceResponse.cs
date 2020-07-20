namespace ReferenceService.WebApi.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    /// <summary>
    /// Ответ при создании справки
    /// </summary>
    public class CreateReferenceResponse : AdditionalInformation
    {
        /// <summary>
        /// Номер справки
        /// </summary>
        [JsonProperty("refNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
    }
}
