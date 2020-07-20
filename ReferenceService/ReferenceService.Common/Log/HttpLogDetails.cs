namespace ReferenceService.Common.Log
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;
    
    /// <summary>
    /// Дополнительная информация при логировании для HTTP
    /// </summary>
    public class HttpLogDetails
    {
        /// <summary>
        /// Конструктор для <see cref="HttpLogDetails"/>
        /// </summary>
        /// <param name="headers">HTTP-заголовки</param>
        public HttpLogDetails(IDictionary<string, string> headers)
        {
            Headers = headers;
        }

        /// <summary>
        /// HTTP-заголовки
        /// </summary>
        [JsonProperty("headers")]
        public IDictionary<string, string> Headers { get; private set; }

        /// <summary>
        /// Тело запроса
        /// </summary>
        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public object Body { get; set; }

        /// <summary>
        /// URL запроса
        /// </summary>
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string URL { get; set; }

        /// <summary>
        /// Метод запроса
        /// </summary>
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public string Method { get; set; }

        /// <summary>
        /// HTTP-код ответа
        /// </summary>
        [JsonProperty("statusCode", NullValueHandling = NullValueHandling.Ignore)]
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Конвертирует HTTP-заголовки в словарь
        /// </summary>
        /// <param name="headers">HTTP-заголовки</param>
        /// <returns>Cловарь</returns>
        public static IDictionary<string, string> ConvertHeaders(HttpHeaders headers)
        {
            var convHeaders = new Dictionary<string, string>(headers.Count());
            foreach (var header in headers)
            {
                convHeaders.Add(header.Key, string.Join(";", header.Value));
            }
            return convHeaders;
        }
    }
}
