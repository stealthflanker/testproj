using NLog;
using ReferenceService.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReferenceService.WebApi.ServiceClients
{
    public class MessageInspectorBehavior : IEndpointBehavior, IClientMessageInspector
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly string _serviceName;
        private readonly string _id;

        public MessageInspectorBehavior(string serviceName, string id)
        {
            _serviceName = serviceName;
            _id = id;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            Log(LogLevel.Info, _serviceName + "_RESPONSE_BODY", new { response = reply.ToString() });
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var to = channel.Via != null ? channel.Via.ToString() : (channel.RemoteAddress != null ? channel.RemoteAddress.ToString() : string.Empty);

            Log(LogLevel.Info, _serviceName + "_REQUEST_BODY", new { to, request = request.ToString() });
            return null;
        }

        /// <summary>
        /// Лог
        /// </summary>
        /// <param name="level">Уровень логирования</param>
        /// <param name="message">Сообщение</param>
        /// <param name="id">Идентификатор</param>
        /// <param name="details">Дополнительная информация</param>
        private void Log(LogLevel level, string message, object details = null)
        {
            LogHelper.Log(_logger, level, message, _id, details);
        }
    }
}
