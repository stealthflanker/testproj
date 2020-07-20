namespace ReferenceService.WebApi.Common
{
    using System;
    using Models;
    
    /// <summary>
    /// Исключение для ReferenceService
    /// </summary>
    public class ReferenceServiceException : Exception
    {
        /// <summary>
        /// Конструктор ReferenceServiceException
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <param name="message">Текст ошибки</param>
        /// <param name="details">Детальная информация об ошибке</param>
        public ReferenceServiceException(int code, string message, string details = null)
            : base(message)
        {
            Code = code;
            Details = details;
        }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Детальная информация об ошибке
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// Преобразовать в массив ошибок <see cref="Error"/>
        /// </summary>
        /// <returns>Массив ошибок</returns>
        public Error[] ToErrors()
        {
            return new[]
            {
                new Error
                {
                    Code = Code.ToString(),
                    InternalMessage = Message,
                }
            };
        }
    }
}
