using LogCorner.EduSync.SignalR.Common;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class ProducerServiceUnitTest
    {
        [Fact]
        public void DoWorkShouldRaiseReceivedOnPublishToTopicEvent()
        {
            var mockNotifier = new Mock<ISignalRNotifier>();

            mockNotifier.Setup(n => n.OnPublish(It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Raises((mock => mock.ReceivedOnPublishToTopic
                        += (topic, @event) =>
                        {
                            var x = topic;
                        }
                    ));

            var mockSignalRPublisher = new Mock<ISignalRPublisher>();
            mockSignalRPublisher.Setup(p => p.SubscribeAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            IProducerService producerService = new ProducerService(mockNotifier.Object, mockSignalRPublisher.Object, It.IsAny<IServiceBus>());

            producerService.DoWork();
        }
    }
}