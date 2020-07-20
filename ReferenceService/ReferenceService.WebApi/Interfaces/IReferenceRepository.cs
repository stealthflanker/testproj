namespace ReferenceService.WebApi.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    
    /// <summary>
    /// Интерфейс репозитория справок
    /// </summary>
    public interface IReferenceRepository
    {
        /// <summary>
        /// Создать справку
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Справка <see cref="Reference"/></returns>
        Task<Reference> Create(Reference reference, CancellationToken cancellationToken);

        /// <summary>
        /// Предоставляет справку по номеру
        /// </summary>
        /// <param name="refNumber">Номер справки</param>
        /// /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Сущность справки</returns>
        Task<Reference> Get(string refNumber, CancellationToken cancellationToken);

        /// <summary>
        /// Обновить параметры справки
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        Task Update(Reference reference, CancellationToken cancellationToken);

        /// <summary>
        /// Поиск справок по условию
        /// </summary>
        /// <param name="predicate">Условие</param>
        /// <returns>Список справок</returns>
        Task<IEnumerable<Reference>> Find(Expression<Func<Reference, bool>> predicate);
    }
}
