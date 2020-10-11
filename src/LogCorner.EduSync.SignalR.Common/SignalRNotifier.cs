using LogCorner.EduSync.Speech.SharedKernel.Events;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class SignalRNotifier : ISignalRNotifier
    {
        public event Action<EventStore> ReceivedOnPublish;

        public event Action<string, EventStore> ReceivedOnPublishToTopic;

        private readonly IHubInstance _hubInstance;

        public SignalRNotifier(IHubInstance hubInstance)
        {
            _hubInstance = hubInstance;
        }

        public async Task StartAsync()
        {
            await _hubInstance.Connection.StartAsync();
        }

        public async Task OnPublish()
        {
            _hubInstance.Connection.On<EventStore>(nameof(IHubNotifier<EventStore>.OnPublish),
                u => ReceivedOnPublish?.Invoke(u));
            await Task.CompletedTask;
        }

        public async Task OnPublish(string topic)
        {
            _hubInstance.Connection.On<string, EventStore>(nameof(IHubNotifier<string>.OnPublish), 
                (u, v) => ReceivedOnPublishToTopic?.Invoke(u, v));
            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            await _hubInstance.Connection.StopAsync();
        }
    }
}


