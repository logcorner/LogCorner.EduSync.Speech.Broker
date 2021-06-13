#nullable enable
using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LogCorner.EduSync.SignalR.Server.Hubs
{
    [Authorize]
    public class LogCornerHub<T> : Hub<IHubNotifier<T>>, IHubInvoker<T> where T : class
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"OnConnectedAsync :: client : {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"OnDisconnectedAsync :: client : {Context.ConnectionId}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Publish(T payload)
        {
            await Clients.All.OnPublish(payload);
            Console.WriteLine($"Publish :: payload : {payload} , client : {Context.ConnectionId}");
        }

        public async Task PublishToTopic(string topic, T payload)
        {
            await Clients.All.OnPublish(topic, payload);
            Console.WriteLine($"PublishToTopic :: topic : {topic} , payload : {payload}, client : {Context.ConnectionId}");
        }

        public async Task Subscribe(string topic)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnSubscribe(Context.ConnectionId, topic);
            Console.WriteLine($"Subscribe :: topic : {topic} , client : {Context.ConnectionId}");
        }

        public async Task UnSubscribe(string topic)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnUnSubscribe(Context.ConnectionId, topic);
            Console.WriteLine($"UnSubscribe :: topic : {topic} , client : {Context.ConnectionId}");
        }
    }
}