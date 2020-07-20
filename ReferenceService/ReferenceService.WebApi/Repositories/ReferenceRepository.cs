namespace ReferenceService.WebApi.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    
    /// <summary>
    /// Репозиторий справок
    /// </summary>
    public class ReferenceRepository : IReferenceRepository
    {
        public class Context : DbContext
        {
            public Context()
                : base("Default")               
            {
            }

            public DbSet<Reference> References { get; set; }
        }

        private Context _db;

        /// <summary>
        /// Создать экземпляр <see cref="ReferenceRepository"/>
        /// </summary>
        public ReferenceRepository()
        {
            _db = new Context();
        }

        /// <summary>
        /// Создать справку
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        public async Task<Reference> Create(Reference reference, CancellationToken cancellationToken)
        {
            var refNumber = await _db.Database.SqlQuery<string>(
                "SELECT \"sch_reference\".\"create_reference\"({0}::VARCHAR,{1}::VARCHAR,{2}::VARCHAR,{3}::VARCHAR,{4}::VARCHAR,{5}::VARCHAR,{6}::VARCHAR,{7}::TIMESTAMPTZ,{8}::VARCHAR,{9}::VARCHAR,{10}::VARCHAR,{11}::VARCHAR,{12}::VARCHAR,{13}::VARCHAR,{14}::VARCHAR,{15}::VARCHAR,{16}::VARCHAR,{17}::VARCHAR,{18}::VARCHAR,{19}::TEXT,{20}::TIMESTAMP);",
                reference.Status,
                reference.Code,
                reference.Name,
                reference.RegistrationChannel,
                reference.DeliveryMethod,
                reference.DeliveryEmail,
                reference.DeliveryBONumber,
                reference.UserDateTime,
                reference.UserID,
                reference.UserFIO,
                reference.UserWorkstationIP,
                reference.UserWorkstationName,
                reference.CustomerCUID,
                reference.CustomerFIO,
                reference.CustomerPassportSerNum,
                DateTime.Now
            ).FirstAsync();

            return await Get(refNumber, cancellationToken);
        }

        /// <summary>
        /// Предоставляет справку по номеру
        /// </summary>
        /// <param name="refNumber">Номер справки</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Сущность справки</returns>
        public async Task<Reference> Get(string refNumber, CancellationToken cancellationToken)
        {
            return await _db.References.SingleOrDefaultAsync(r => r.Number == refNumber, cancellationToken);
        }

        /// <summary>
        /// Обновить параметры справки
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        public async Task Update(Reference reference, CancellationToken cancellationToken)
        {
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Поиск справок по условию
        /// </summary>
        /// <param name="predicate">Условие</param>
        /// <returns>Список справок</returns>
        public async Task<IEnumerable<Reference>> Find(Expression<Func<Reference, bool>> predicate)
        {
            return await _db.References.Where(predicate).ToListAsync();
        }
    }
}
