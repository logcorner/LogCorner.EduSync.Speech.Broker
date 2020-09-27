using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class SignalRPublisher : ISignalRPublisher
    {
        private readonly IHubInstance _hubConnectionInstance;

        public SignalRPublisher(IHubInstance hubConnectionInstance)
        {
            _hubConnectionInstance = hubConnectionInstance;
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
            await _hubConnectionInstance.Connection.InvokeAsync(nameof(IHubInvoker<object>.PublishToTopic), topic, payload);
        }
    }
}