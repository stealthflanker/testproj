namespace ReferenceService.WebApi.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    /// <summary>
    /// Интерфейс клиента для PrintServer
    /// </summary>
    public interface IPrintServerBridge
    {
        /// <summary>
        /// Формирует справку в PrintServer
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="refParams">Параметры справки для PrintServer</param>
        /// <param name="id">Идентификатор запроса</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        Task Store(Reference reference, IDictionary<string, string> refParams, string id, CancellationToken cancellationToken);
    }
}
