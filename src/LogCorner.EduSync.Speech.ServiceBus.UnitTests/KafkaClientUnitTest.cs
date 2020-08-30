using Confluent.Kafka;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class KafkaClientUnitTest
    {
        [Fact]
        public async Task KafkaClientShouldSendMessageToKafkaServer()
        {
            //Arrange
            var mockProducer = new Mock<IProducer<Null, string>>();
            mockProducer.Setup(m => m.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), CancellationToken.None));
            var mockIJsonSerializer = new Mock<IJsonSerializer>();

            string @event = "jjsonString";
            mockIJsonSerializer.Setup(m => m.Serialize(It.IsAny<EventStore>())).Returns(@event);

            //Act
            IKafkaClient kafkaClient = new KafkaClient(mockProducer.Object, mockIJsonSerializer.Object);
            await kafkaClient.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>());

            //Assert
            mockProducer.Verify(m => m.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task ___KafkaClientShouldSendMessageToKafkaServer()
        {
            //Arrange
            var mockProducer = new Mock<IProducer<Null, string>>();
            var t = mockProducer.Setup(m =>
                      m.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), CancellationToken.None))
                          /*.Callback(
                              () =>
                              {
                                  throw new Exception();
                              })*/;

            var mockIJsonSerializer = new Mock<IJsonSerializer>();

            string @event = "jjsonString";
            mockIJsonSerializer.Setup(m => m.Serialize(It.IsAny<EventStore>())).Returns(@event);

            //Act
            IKafkaClient kafkaClient = new KafkaClient(mockProducer.Object, mockIJsonSerializer.Object);
            await kafkaClient.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>());

            //Assert
            mockProducer.Verify(m => m.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), CancellationToken.None), Times.Once);
        }
    }
}