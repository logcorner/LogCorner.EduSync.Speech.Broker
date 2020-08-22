using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Server.Hubs
{
    public class LogCornerHub<T> : Hub<IHubNotifier<T>>, IHubInvoker<T> where T : class
    {
        public async Task Publish(T payload)
        {
            await Clients.All.OnPublish(payload);
        }

        public async Task PublishToTopic(string topic, T payload)
        {
            await Clients.All.OnPublish(topic, payload);
        }

        public async Task Subscribe(string topic)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnSubscribe(Context.ConnectionId, topic);
        }

        public async Task UnSubscribe(string topic)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnUnSubscribe(Context.ConnectionId, topic);
        }
    }
}