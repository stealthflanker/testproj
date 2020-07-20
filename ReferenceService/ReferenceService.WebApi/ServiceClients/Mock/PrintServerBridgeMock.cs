namespace ReferenceService.WebApi.ServiceClients.Mock
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Entities;
    using Interfaces;
    
    /// <summary>
    /// Mock для PrintServer
    /// </summary>
    public class PrintServerBridgeMock : IPrintServerBridge
    {
        /// <summary>
        /// Формирует справку в PrintServer
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="refParams">Параметры справки для  PrintServer</param>
        /// <param name="id">Идентификатор запроса</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        public Task Store(Reference reference, IDictionary<string, string> refParams, string id, CancellationToken cancellationToken)
        {
            if(reference.UserFIO == "ошибку вернет запрос")
            {
                throw new ReferenceServiceException(100, "ERROR_TEST_OK");
            }
            return Task.FromResult(0);
        }
    }
}
