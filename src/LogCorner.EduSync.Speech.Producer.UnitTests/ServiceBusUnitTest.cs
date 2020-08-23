using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class ServiceBusUnitTest
    {
        [Fact]
        public async Task Should_receive_messages_to_produce()
        {
            //Arrange
            var eventStore = new EventStore
            {
                Id = "1"
            };
            IHubConnectionInstance hubConnectionInstance = new HubConnectionInstanceMock();

            await hubConnectionInstance.ConnectAsync();
            IServiceBus bus = new ServiceBus(hubConnectionInstance);

            await bus.ReceiveAsync();

            //Act
            //Assert
            bus.ReceivedOnPublish += value =>
            {
                Assert.Equal(eventStore, value);
            };

            await hubConnectionInstance.Connection.StartAsync();

            await hubConnectionInstance.Connection.InvokeAsync(nameof(IHubInvoker<EventStore>.Publish
            ), eventStore);
        }
    }
}