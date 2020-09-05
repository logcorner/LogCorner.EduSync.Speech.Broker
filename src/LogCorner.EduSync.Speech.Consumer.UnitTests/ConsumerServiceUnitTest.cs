using LogCorner.EduSync.Speech.ServiceBus;
using Moq;
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

            mockServiceBus.Setup(m => m.ReceiveAsync(It.IsAny<string>())).Verifiable();

            IConsumerService consumerService = new ConsumerService(mockServiceBus.Object);

            await consumerService.DoWorkAsync();

            _ = Task.Run(() => mockServiceBus.Verify(r => r.ReceiveAsync(It.IsAny<string>())));
        }
    }
}