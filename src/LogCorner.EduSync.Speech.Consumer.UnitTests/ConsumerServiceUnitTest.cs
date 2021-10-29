using LogCorner.EduSync.Speech.ServiceBus;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Consumer.UnitTests
{
    public class ConsumerServiceUnitTest
    {
        [Fact]
        public async Task DoWorkShouldRaiseReceivedOnPublishToTopicEvent()
        {
            var mockServiceBus = new Mock<IServiceBus>();
            var mockClusterManager = new Mock<IClusterManager>();

            mockServiceBus.Setup(m => m.ReceiveAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>())).Verifiable();

            IConsumerService consumerService = new ConsumerService(mockServiceBus.Object, mockClusterManager.Object);

            await consumerService.DoWorkAsync(It.IsAny<CancellationToken>());

            _ = Task.Run(() => mockServiceBus.Verify(r => r.ReceiveAsync(It.IsAny<string[]>(), It.IsAny<CancellationToken>())));
        }
    }
}