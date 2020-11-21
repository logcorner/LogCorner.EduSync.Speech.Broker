using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
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
            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRPublisher signalRPublisher = new SignalRPublisher(hubConnectionInstance, It.IsAny<IJsonSerializer>());

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
            string payload = "payload";
            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();
            var mockJsonSerializer = new Mock<IJsonSerializer>();
            mockJsonSerializer.Setup(m => m.Serialize(It.IsAny<object>())).Returns("");
            ISignalRPublisher signalRPublisher = new SignalRPublisher(hubConnectionInstance, mockJsonSerializer.Object);

            //Act
            await signalRPublisher.PublishAsync(topic, payload);

            //Assert
            Assert.Equal(HubConnectionState.Connected, hubConnectionInstance.Connection.State);
        }
    }
}