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

        private readonly IHubConnectionInstance _hubConnectionInstance;

        public SignalRNotifier(IHubConnectionInstance hubConnectionInstance)
        {
            _hubConnectionInstance = hubConnectionInstance;
        }

        public async Task OnPublish()
        {
            _hubConnectionInstance.Connection.On<EventStore>(nameof(IHubNotifier<EventStore>.OnPublish), u => ReceivedOnPublish?.Invoke(u));
            await Task.CompletedTask;
        }

        public async Task OnPublish(string topic)
        {
            _hubConnectionInstance.Connection.On<string, EventStore>(nameof(IHubNotifier<string>.OnPublish), (u, v) => ReceivedOnPublishToTopic?.Invoke(u, v));
            await Task.CompletedTask;
        }

        public async Task Begin()
        {
            await _hubConnectionInstance.Connection.StartAsync();
        }
    }
}