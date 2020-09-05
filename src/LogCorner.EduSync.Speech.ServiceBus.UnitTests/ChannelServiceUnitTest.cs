using Moq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class ChannelServiceUnitTest
    {
        [Fact]
        public void ServiceBusShouldSendMessageToKafka()
        {
            //Arrange
            var channel = Channel.CreateUnbounded<string>();
            string data = "test";

            //Act
            IChannelService channelService = new ChannelService();
            var task = channelService.Produce(channel.Writer, It.IsAny<int>(), data);

            //Assert
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void ChannelServiceShouldConsume()
        {
            //Arrange
            var channel = Channel.CreateUnbounded<string>();

            //Act
            IChannelService channelService = new ChannelService();
            var producer = channelService.Consume(channel.Reader, It.IsAny<int>());
            var consumer = channelService.Produce(channel.Writer, It.IsAny<int>(), "data");

            //Assert
            Assert.Equal(TaskStatus.WaitingForActivation, producer.Status);
            Assert.Equal(TaskStatus.RanToCompletion, consumer.Status);
        }
    }
}