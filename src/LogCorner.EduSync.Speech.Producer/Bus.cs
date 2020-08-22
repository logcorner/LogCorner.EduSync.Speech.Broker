using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.SignalR.Client;
using System;

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

        public void ReceiveAsync()
        {
            _hubConnectionInstance.Connection.On<EventStore>(nameof(IHubNotifier<EventStore>.OnPublish), u => ReceivedOnPublish?.Invoke(u));
        }
    }
}