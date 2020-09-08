using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Consumer.UnitTests
{
    public class ConsumerHostedServiceUnitTest
    {
        [Fact]
        public void ShouldStartConsumer()
        {
            //Arrange
            var mockConsumerService = new Mock<IConsumerService>();
            var consumerHostedService = new ConsumerHostedService(mockConsumerService.Object);

            //Act
            var task = consumerHostedService.StartAsync(It.IsAny<CancellationToken>());

            //Assert

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public void ShouldStopConsumer()
        {
            //Arrange
            var mockConsumerService = new Mock<IConsumerService>();
            var consumerHostedService = new ConsumerHostedService(mockConsumerService.Object);

            //Act
            var task = consumerHostedService.StopAsync(It.IsAny<CancellationToken>());

            //Assert

            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
        }

        [Fact]
        public async Task ShouldExecuteConsumer()
        {
            //Arrange
            var mockConsumerService = new Mock<IConsumerService>();
            mockConsumerService.Setup(m => m.DoWorkAsync()).Verifiable();
            var consumerHostedService = new ConsumerHostedServiceMock(mockConsumerService.Object);

            //Act
            await consumerHostedService.ExecuteAsync(It.IsAny<CancellationToken>());

            //Assert
            mockConsumerService.Verify(m => m.DoWorkAsync(), Times.Once);
        }
    }
}