using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.SignalR.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.SignalR.Server.IntegrationTests
{
    public class LogCornerHubIntegrationTests
    {
        private event Action<string> ReceivedOnPublish;

        private event Action<string, string> ReceivedOnSubscribe;

        private event Action<string, string> ReceivedOnPublishToTopic;

        [Fact]
        public async Task Publish_with_the_same_message_when_invoke_then_OnPublish_should_receive_the_same_message()
        {
            string payloadToSend = "test";
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
                    });
                });

            var server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/logcornerhub",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();

            connection.On<string>(nameof(IHubNotifier<string>.OnPublish), u => ReceivedOnPublish?.Invoke(u));

            await connection.StartAsync();
            await connection.InvokeAsync(nameof(IHubInvoker<string>.Publish
            ), payloadToSend);

            ReceivedOnPublish += value =>
            {
                
                Assert.Equal(payloadToSend, value);
            };
        }

        [Fact]
        public async Task Subscribe_with_the_same_topic_when_invoke_then_OnSubscribe_should_subscribe_to_the_same_topic()
        {
            string topicToSubscribe = "orders";
            var webHostBuilder = new WebHostBuilder()
                  .ConfigureServices(services =>
                  {
                      services.AddSignalR();
                  })
                  .Configure(app =>
                  {
                      app.UseRouting();
                      app.UseEndpoints(endpoints =>
                      {
                          endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
                      });
                  });

            var server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/logcornerhub",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();

            connection.On<string, string>(nameof(IHubNotifier<string>.OnSubscribe), (u, v) => ReceivedOnSubscribe?.Invoke(u, v));

            await connection.StartAsync();
            await connection.InvokeAsync(nameof(IHubInvoker<string>.Subscribe), topicToSubscribe);

            ReceivedOnSubscribe += (connectionId, topic) =>
            {
                Assert.False(string.IsNullOrWhiteSpace(connectionId));
                Assert.Equal(topicToSubscribe, topic);
            };
        }

        [Fact]
        public async Task UnSubscribe_with_the_same_topic_when_invoke_then_OnUnSubscribe_should_unsubscribe_to_the_same_topic()
        {
            string topicToSubscribe = "orders";
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
                    });
                });

            var server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/logcornerhub",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();

            connection.On<string, string>(nameof(IHubNotifier<string>.OnUnSubscribe), (u, v) => ReceivedOnSubscribe?.Invoke(u, v));

            await connection.StartAsync();
            await connection.InvokeAsync(nameof(IHubInvoker<string>.UnSubscribe), topicToSubscribe);

            ReceivedOnSubscribe += (connectionId, topic) =>
            {
                Assert.False(string.IsNullOrWhiteSpace(connectionId));
                Assert.Equal(topicToSubscribe, topic);
            };
        }

        [Fact]
        public async Task PublishToTopic_with_the_same_message_when_invoke_then_OnPublishToTopic_should_receive_the_same_message()
        {
            string payloadToSend = "test";
            string topicToSubscribe = "topic";
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
                    });
                });

            var server = new TestServer(webHostBuilder);
            var connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/logcornerhub",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();

            connection.On<string, string>(nameof(IHubNotifier<string>.OnPublish), (u, v) => ReceivedOnPublishToTopic?.Invoke(u, v));

            await connection.StartAsync();

            await connection.InvokeAsync(nameof(IHubInvoker<string>.Subscribe), topicToSubscribe);
            await connection.InvokeAsync(nameof(IHubInvoker<string>.PublishToTopic), topicToSubscribe, payloadToSend);

            ReceivedOnPublishToTopic += (topic, value) =>
            {
                Assert.Equal(topicToSubscribe, topic);
                Assert.Equal(payloadToSend, value);
            };
        }
    }
}