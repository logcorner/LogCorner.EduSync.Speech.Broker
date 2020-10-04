using System.Threading;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class ServiceBusUnitTest
    {
        [Fact]
        public async Task ServiceBusShouldSendMessageToKafka()
        {
            //Arrange
            var mockKafkaClient = new Mock<IServiceBusProvider>();
            mockKafkaClient.Setup(m => m.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>())).Verifiable();

            //Act
            IServiceBus serviceBus = new ServiceBus(mockKafkaClient.Object);
            await serviceBus.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>());

            //Assert
            mockKafkaClient.Verify(m => m.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>()), Times.Once);
        }

        [Fact]
        public async Task ServiceBusShouldReceiveMessageFromKafka()
        {
            //Arrange
            string topic = "bus";
            var mockKafkaClient = new Mock<IServiceBusProvider>();
            mockKafkaClient.Setup(m => m.ReceiveAsync(topic, It.IsAny<CancellationToken>(),true)).Verifiable();

            //Act
            IServiceBus serviceBus = new ServiceBus(mockKafkaClient.Object);
            await serviceBus.ReceiveAsync(topic, It.IsAny<CancellationToken>());

            //Assert
            mockKafkaClient.Verify(m => m.ReceiveAsync(topic, It.IsAny<CancellationToken>(), true), Times.Once);
        }
    }
}