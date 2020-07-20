namespace ReferenceService.WebApi.ServiceClients
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using Entities;
    using Interfaces;
    using NLog;
    using ReferenceService.Common;
    using ReferenceService.Common.Log;
    using System.Xml;
    /// <summary>
    /// Клиент для  PrintServer
    /// </summary>
    public class PrintServerBridge : IPrintServerBridge
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Создать клиент для  PrintServer
        /// </summary>
        /// <param name="id">Идентификатор запроса</param>
        /// <returns>Клиент  PrintServer <see cref="PrintServerBridgeWS.PrintServerBridgeWSClient"/></returns>
        private static PrintServerBridgeWS.PrintServerBridgeWSClient CreateClient(string id)
        {
            var client = new PrintServerBridgeWS.PrintServerBridgeWSClient("PrintServerBridgeWSSOAP");

            client.Endpoint.EndpointBehaviors.Add(new MessageInspectorBehavior("PrintServerBridge", id));

            var credentials = ReferenceServiceConfiguration.Instance.PrintServer;
            client.ClientCredentials.UserName.UserName = credentials.UserName;
            client.ClientCredentials.UserName.Password = credentials.Password;

            return client;
        }

        /// <summary>
        /// Формирует справку в  PrintServer
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <param name="refParams">Параметры справки для  PrintServer</param>
        /// <param name="id">Идентификатор запроса</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/></param>
        /// <returns>Task <see cref="Task"/></returns>
        public async Task Store(Reference reference, IDictionary<string, string> refParams, string id, CancellationToken cancellationToken)
        {
            var parameters = new List<PrintServerBridgeWS.ReportEntry>();
            foreach(var refParam in refParams)
            {
                parameters.Add(new PrintServerBridgeWS.ReportEntry
                {
                    key = refParam.Key,
                    value = refParam.Value
                });
            }

            var props = new PrintServerBridgeWS.ReportProperties
            {
                numberOfCopies = 1,
                parameters = parameters.ToArray(),
                reportCode = "WPA_REF_" + reference.Code
            };

            Log(LogLevel.Info, "PrintServerBridge_REQUEST", id, props);

            var client = CreateClient(id);

            var response = await client.storeAsync(reference.Number, props);

            bool isCalculatorAttribute = false;
            if (response != null && response.serviceResult != null && response.serviceResult.resultDetail != null)
            {
                var processNotice = response.serviceResult.resultDetail as XmlNode[];
                if (processNotice != null && processNotice.Length > 0)
                {
                    foreach (var node in processNotice[0].ChildNodes)
                    {
                        var elem = node as XmlElement;
                        if (elem != null && elem.Name == "attribute" && elem.InnerText != null)
                        {
                            isCalculatorAttribute = elem.InnerText.ToLower() == "calculator";
                            break;
                        }
                    }
                }                

            }

            if (response.serviceResult.resultType == PrintServerBridgeWS.resultTypeEnum.ERROR)
            {
                Log(LogLevel.Error, "PrintServerBridge_RESPONSE", id, response);
                                
                if (isCalculatorAttribute)
                {
                    throw new ReferenceServiceException(102, Resources.Strings.ReferenceServiceError102, response.serviceResult.resultText);
                }
                else
                    throw new ReferenceServiceException(100, Resources.Strings.ReferenceServiceError100, response.serviceResult.resultText);
            } else
            {
                Log(LogLevel.Info, "PrintServerBridge_RESPONSE", id, response);
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
