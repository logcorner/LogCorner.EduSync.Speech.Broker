using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class NotifierMediatorServiceUnitTest
    {
        [Fact]
        public async Task NotifierMediatorServiceWithNullMessageShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>())).Verifiable();

            //Act
            INotifierMediatorService notifierMediatorService = new NotifierMediatorService(mockMediator.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => notifierMediatorService.Notify<object>(null));
        }

        [Fact]
        public async Task NotifierMediatorServiceShouldSendNotifications()
        {
            //Arrange
            var message = new NotificationMessage<string> { Message = "notifyText" };

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>())).Verifiable();

            //Act
            INotifierMediatorService notifierMediatorService = new NotifierMediatorService(mockMediator.Object);
            await notifierMediatorService.Notify(message);

            //Assert
            mockMediator.Verify(m => m.Publish(message, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}