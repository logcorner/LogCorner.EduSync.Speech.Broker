using Confluent.Kafka;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using Moq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
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

            string @event = "jsonString";
            mockIJsonSerializer.Setup(m => m.Serialize(It.IsAny<EventStore>())).Returns(@event);

            //Act
            IKafkaClient kafkaClient = new KafkaClient(mockProducer.Object, mockIJsonSerializer.Object, null, null);
            await kafkaClient.SendAsync(It.IsAny<string>(), It.IsAny<EventStore>());

            //Assert
            mockProducer.Verify(m => m.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task KafkaClientShouldReceiveMessageFromKafkaServer()
        {
            //Arrange
            var mockConsumer = new Mock<IConsumer<Null, string>>();
            mockConsumer.Setup(m => m.Consume(CancellationToken.None)).Returns(new ConsumeResult<Null, string>()
            {
                Message = new Message<Null, string>()
                {
                    Value = "test"
                }
            });
            var mockNotifierMediatorService = new Mock<INotifierMediatorService>();
           
            //Act
            IKafkaClient kafkaClient = new KafkaClient(It.IsAny<IProducer<Null, string>>(), It.IsAny<IJsonSerializer>(), mockConsumer.Object, mockNotifierMediatorService.Object);
            await kafkaClient.ReceiveAsync("topic", It.IsAny<CancellationToken>(),false);

            //Assert
            mockConsumer.Verify(m => m.Consume(CancellationToken.None), Times.Once);
        }
    }
}