using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using LogCorner.EduSync.SignalR.Common.Model;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;

namespace LogCorner.EduSync.SignalR.Common
{
    public class SignalRPublisher : ISignalRPublisher
    {
        private readonly IHubInstance _hubConnectionInstance;
        private readonly IJsonSerializer _eventSerializer;

        public SignalRPublisher(IHubInstance hubConnectionInstance , IJsonSerializer eventSerializer)
        {
            _hubConnectionInstance = hubConnectionInstance;
            _eventSerializer = eventSerializer;
        }

        public async Task SubscribeAsync(string topic)
        {
            if (_hubConnectionInstance.Connection.State != HubConnectionState.Connected)
            {
                await _hubConnectionInstance.StartAsync();
            }
            await _hubConnectionInstance.Connection.InvokeAsync(nameof(IHubInvoker<string>.Subscribe), topic);
        }

        public async Task PublishAsync<T>(string topic, T payload)
        {
            if (_hubConnectionInstance.Connection.State != HubConnectionState.Connected)
            {
                await _hubConnectionInstance.StartAsync();
            }

            var serializedBody = _eventSerializer.Serialize(payload);

            var type = payload.GetType().AssemblyQualifiedName;
            var message = new Message(type, serializedBody);

            await _hubConnectionInstance.Connection.InvokeAsync(nameof(IHubInvoker<Message>.PublishToTopic), topic, message);
        }
    }
}