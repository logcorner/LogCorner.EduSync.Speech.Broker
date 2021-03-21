using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class ProducerServiceUnitTest
    {
        [Fact]
        public async Task DoWorkShouldRaiseReceivedOnPublishToTopicEvent()
        {
            var mockServiceBus = new Mock<IServiceBus>();

            mockServiceBus.Setup(m => m.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>())).Verifiable();
            var payLoad = new EventStore(It.IsAny<Guid>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>());
            var mockJsonSerializer = new Mock<IJsonSerializer>();
            mockJsonSerializer.Setup(m => m.Serialize(It.IsAny<object>())).Returns("");
            IHubInstance connectionInstance = new HubConnectionInstanceMock();
            await connectionInstance.InitAsync();
            ISignalRNotifier notifier = new SignalRNotifier(connectionInstance, mockJsonSerializer.Object);
            ISignalRPublisher publisher = new SignalRPublisher(connectionInstance, mockJsonSerializer.Object);

            IProducerService producerService = new ProducerService(notifier, publisher, mockServiceBus.Object);
            await notifier.StartAsync();
            await producerService.DoWorkAsync();

            await publisher.PublishAsync(Topics.Speech, payLoad);

            _ = Task.Run(() => mockServiceBus.Verify(r => r.SendAsync(Topics.Speech, payLoad)));
        }

        [Fact]
        public async Task ShouldStartSignalRNotifier()
        {
            //Arrange
            var moqISignalRNotifier = new Mock<ISignalRNotifier>();
            moqISignalRNotifier.Setup(m => m.StartAsync()).Verifiable();

            IProducerService producerService = new ProducerService(moqISignalRNotifier.Object, It.IsAny<ISignalRPublisher>(), It.IsAny<IServiceBus>());

            //Act
            await producerService.StartAsync();

            //Assert
            moqISignalRNotifier.Verify(r => r.StartAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldStopSignalRNotifier()
        {
            //Arrange
            var moqISignalRNotifier = new Mock<ISignalRNotifier>();
            moqISignalRNotifier.Setup(m => m.StopAsync()).Verifiable();

            IProducerService producerService = new ProducerService(moqISignalRNotifier.Object, It.IsAny<ISignalRPublisher>(), It.IsAny<IServiceBus>());

            //Act
            await producerService.StopAsync();

            //Assert
            moqISignalRNotifier.Verify(r => r.StopAsync(), Times.Once);
        }
    }
}