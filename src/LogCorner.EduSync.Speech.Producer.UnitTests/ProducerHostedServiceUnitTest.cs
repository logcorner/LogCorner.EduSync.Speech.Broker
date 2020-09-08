using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class ProducerHostedServiceUnitTest
    {
        [Fact]
        public async Task ShouldStartProducer()
        {
            //Arrange
            var mockProducerService = new Mock<IProducerService>();
            mockProducerService.Setup(m => m.DoWorkAsync()).Verifiable();
            var producerHostedService = new ProducerHostedService(mockProducerService.Object);

            //Act
            await producerHostedService.StartAsync(It.IsAny<CancellationToken>());

            //Assert

            mockProducerService.Verify(m => m.StartAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldStopProducer()
        {
            //Arrange
            var mockProducerService = new Mock<IProducerService>();
            mockProducerService.Setup(m => m.DoWorkAsync()).Verifiable();
            var producerHostedService = new ProducerHostedService(mockProducerService.Object);

            //Act
            await producerHostedService.StopAsync(It.IsAny<CancellationToken>());

            //Assert
            mockProducerService.Verify(m => m.StopAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldExecuteProducer()
        {
            //Arrange
            var mockProducerService = new Mock<IProducerService>();
            mockProducerService.Setup(m => m.DoWorkAsync()).Verifiable();
            var producerHostedService = new ProducerHostedServiceMock(mockProducerService.Object);

            //Act
            await producerHostedService.ExecuteAsync(It.IsAny<CancellationToken>());

            //Assert
            mockProducerService.Verify(m => m.DoWorkAsync(), Times.Once);
        }
    }
}