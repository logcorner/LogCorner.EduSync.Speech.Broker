using System;
using Moq;
using System.Threading.Channels;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
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
            IChannelService channelService = new ChannelService(It.IsAny<IEventSerializer>(),null);
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
            IChannelService channelService = new ChannelService(It.IsAny<IEventSerializer>(),null);
            var producer = channelService.Consume(channel.Reader, It.IsAny<int>());
            var consumer = channelService.Produce(channel.Writer, It.IsAny<int>(), "data");

            //Assert
            Assert.Equal(TaskStatus.WaitingForActivation, producer.Status);
            Assert.Equal(TaskStatus.RanToCompletion, consumer.Status);
        }
    }
}