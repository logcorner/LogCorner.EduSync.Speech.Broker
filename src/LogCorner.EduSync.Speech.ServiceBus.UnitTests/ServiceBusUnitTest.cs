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
            var mockKafkaClient = new Mock<IKafkaClient>();
            mockKafkaClient.Setup(m => m.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>())).Verifiable();

            //Act
            IServiceBus serviceBus = new ServiceBus(mockKafkaClient.Object);
            await serviceBus.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>());

            //Assert
            mockKafkaClient.Verify(m => m.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>()), Times.Once);
        }
    }
}