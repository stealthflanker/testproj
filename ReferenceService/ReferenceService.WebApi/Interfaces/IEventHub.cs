namespace ReferenceService.WebApi.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    /// <summary>
    /// Интерфейс EventHub
    /// </summary>
    public interface IEventHub
    {
        /// <summary>
        /// Отправляет информацию о справке в EventHub, сервис уведомлений
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="id">Идентификатор запроса</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        Task Send(Reference reference, string id, CancellationToken cancellationToken);
    }
}
