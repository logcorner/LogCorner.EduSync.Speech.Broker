using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.SignalR.Common.IntegrationTests
{
    public class SignalRPublisherIntegrationTest
    {
        [Fact]
        public async Task ShouldSubscribeAsync()
        {
            //Arrange

            string topic = "test";
            IHubConnectionInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRPublisher signalRPublisher = new SignalRPublisher(hubConnectionInstance);

            //Act
            await signalRPublisher.SubscribeAsync(topic);

            //Assert

            Assert.Equal(HubConnectionState.Connected, hubConnectionInstance.Connection.State);
        }

        [Fact]
        public async Task ShouldPublishAsync()
        {
            //Arrange

            string topic = "test";
            IHubConnectionInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRPublisher signalRPublisher = new SignalRPublisher(hubConnectionInstance);

            //Act
            await signalRPublisher.PublishAsync(topic, It.IsAny<object>());

            //Assert

            Assert.Equal(HubConnectionState.Connected, hubConnectionInstance.Connection.State);
        }
    }
}