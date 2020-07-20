namespace ReferenceService.WebApi.ServiceClients.Mock
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Entities;
    using Interfaces;
    
    /// <summary>
    /// Mock для EventHub
    /// </summary>
    public class EventHubMock : IEventHub
    {
        /// <summary>
        /// Отправляет информацию о справке в EventHub
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        public Task Send(Reference reference, string id, CancellationToken cancellationToken)
        {
            if(reference.DeliveryEmail == "error@error.com")
            {
                throw new ReferenceServiceException(100, "ERROR_TEST_OK");
            }

            return Task.FromResult(0);
        }
    }
}
