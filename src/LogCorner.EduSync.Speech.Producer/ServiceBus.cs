using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer
{
    public class ServiceBus : IServiceBus
    {
        public event Action<EventStore> ReceivedOnPublish;

        private readonly IHubConnectionInstance _hubConnectionInstance;

        public ServiceBus(IHubConnectionInstance hubConnectionInstance)
        {
            _hubConnectionInstance = hubConnectionInstance;
        }

        public async Task ReceiveAsync()
        {
            _hubConnectionInstance.Connection.On<EventStore>(nameof(IHubNotifier<EventStore>.OnPublish), u => ReceivedOnPublish?.Invoke(u));
            await Task.CompletedTask;
        }

        public async Task Begin()
        {
            await _hubConnectionInstance.Connection.StartAsync();
        }
    }
}