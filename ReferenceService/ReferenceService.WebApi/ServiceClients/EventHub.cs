namespace ReferenceService.WebApi.ServiceClients
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Entities;
    using EventHubWS;
    using Interfaces;
    using NLog;
    using ReferenceService.Common;
    using ReferenceService.Common.Log;
    using System.Collections.Generic;

    /// <summary>
    /// Клиент EventHub
    /// </summary>
    public class EventHub : IEventHub
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Создать клиент для EventHub
        /// </summary>
        /// <param name="id">Идентификатор запроса</param>
        /// <returns>Клиент EventHub <see cref="EventHubWSClient"/></returns>
        private static EventHubWSClient CreateClient(string id)
        {
            var client = new EventHubWSClient("EventHubWSSOAP");

            client.Endpoint.EndpointBehaviors.Add(new MessageInspectorBehavior("EventHub", id));

            var credentials = ReferenceServiceConfiguration.Instance.EventHub;
            client.ClientCredentials.UserName.UserName = credentials.UserName;
            client.ClientCredentials.UserName.Password = credentials.Password;

            return client;
        }

        /// <summary>
        /// Отправляет информацию о справке в EventHub
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="id">Идентификатор запроса</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        public async Task Send(Reference reference, string id, CancellationToken cancellationToken)
        {
            var details = new List<EventDetail>() 
            {
                new EventDetail
                {
                    parameter = "cuid",
                    value = reference.CustomerCUID
                },
                new EventDetail
                {
                    parameter = "EMAIL_TO",
                    value = reference.DeliveryEmail
                },
                new EventDetail
                {
                    parameter = "reference_name",
                    value = reference.Name
                }
            };

            var request = new sendEventOfflineRequest
            {
                eventTypeId = Constants.EventTypeID,
                eventDate = DateTime.Now,
                eventDetails = details.ToArray()
            };

            Log(LogLevel.Info, "EventHub_REQUEST", id, request);

            var client = CreateClient(id);

            var response = await client.sendEventOfflineAsync(request);

            if(response.serviceResult.resultType == resultTypeEnum.ERROR)
            {
                Log(LogLevel.Error, "EventHub_RESPONSE", id, response);
                throw new ReferenceServiceException(101, Resources.Strings.ReferenceServiceError101, response.serviceResult.resultText);
            } else
            {
                Log(LogLevel.Info, "EventHub_RESPONSE", id, response);
            }
        }

        /// <summary>
        /// Лог
        /// </summary>
        /// <param name="level">Уровень логирования</param>
        /// <param name="message">Сообщение</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="details">Дополнительная информация</param>
        private void Log(LogLevel level, string message, string id, object details = null)
        {
            LogHelper.Log(_logger, level, message, id, details);
        }
    }
}
