using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.SignalR.Common.IntegrationTests
{
    public class SignalRNotifierIntegrationTest
    {
        [Fact]
        public async Task ShouldStart()
        {
            //Arrange

            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRNotifier signalRPublisher = new SignalRNotifier(hubConnectionInstance, It.IsAny<IJsonSerializer>());

            //Act
            var t = signalRPublisher.StartAsync();

            //Assert

            Assert.Equal(TaskStatus.WaitingForActivation, t.Status);
        }

        [Fact]
        public async Task ShouldStop()
        {
            //Arrange

            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRNotifier signalRPublisher = new SignalRNotifier(hubConnectionInstance, It.IsAny<IJsonSerializer>());

            //Act
            var t = signalRPublisher.StopAsync();

            //Assert

            Assert.Equal(TaskStatus.WaitingForActivation, t.Status);
        }

        [Fact]
        public async Task ShouldOnPublishToTopic()
        {
            //Arrange

            string topic = "test";
            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRNotifier signalRPublisher = new SignalRNotifier(hubConnectionInstance, It.IsAny<IJsonSerializer>());

            //Act
            var t = signalRPublisher.OnPublish(topic);

            //Assert

            Assert.Equal(TaskStatus.RanToCompletion, t.Status);
        }

        [Fact]
        public async Task ShouldOnPublish()
        {
            //Arrange

            IHubInstance hubConnectionInstance = new HubConnectionInstanceMock();
            await hubConnectionInstance.InitAsync();

            ISignalRNotifier signalRPublisher = new SignalRNotifier(hubConnectionInstance, It.IsAny<IJsonSerializer>());

            //Act
            var t = signalRPublisher.OnPublish();

            //Assert

            Assert.Equal(TaskStatus.RanToCompletion, t.Status);
        }
    }
}