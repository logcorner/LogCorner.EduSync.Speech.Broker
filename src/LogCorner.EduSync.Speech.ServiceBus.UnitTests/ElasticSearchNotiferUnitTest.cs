using LogCorner.EduSync.Notification.Common.Hub;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using LogCorner.EduSync.Notification.Common;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using Xunit;

namespace LogCorner.EduSync.Speech.ServiceBus.UnitTests
{
    public class ElasticSearchNotiferUnitTest
    {
        [Fact]
        public async Task ElasticSearchNotiferWithNullMessageShouldRaiseArgumentNullException()
        {
            //Arrange
            var mockEventSerializer = new Mock<IEventSerializer>();

            var mockJsonSerializer = new Mock<IJsonSerializer>();

            var mockElasticSearchClient = new Mock<IElasticSearchClient<SpeechProjection>>();

            var mockSignalRPublisher = new Mock<ISignalRPublisher>();

            //Act
            //Assert
            INotificationHandler<NotificationMessage<string>> elasticSearchNotifer = new ElasticSearchNotifer(mockEventSerializer.Object, mockJsonSerializer.Object, mockElasticSearchClient.Object, mockSignalRPublisher.Object);
            await Assert.ThrowsAsync<ArgumentNullException>(() => elasticSearchNotifer.Handle(null, CancellationToken.None));
        }

        [Fact]
        public async Task ElasticSearchNotiferShouldIndexMessage()
        {
            //Arrange
            var eventStore = new EventStore(Guid.NewGuid(),
               It.IsAny<long>(), It.IsAny<string>(), "type", DateTime.Now, "body");
            var message = new NotificationMessage<string> { Message = "notifyText" };

            var @event = new SpeechTitleChangedEvent(Guid.NewGuid(), "title");
            var mockEventSerializer = new Mock<IEventSerializer>();
            mockEventSerializer.Setup(m => m.Deserialize<Event>(It.IsAny<string>(), It.IsAny<string>())).Returns(@event);

            var mockJsonSerializer = new Mock<IJsonSerializer>();
            mockJsonSerializer.Setup(m => m.Deserialize<EventStore>(It.IsAny<string>()))
                .Returns(eventStore);

            var mockElasticSearchClient = new Mock<IElasticSearchClient<SpeechProjection>>();
            mockElasticSearchClient.Setup(m => m.CreateAsync(It.IsAny<SpeechProjection>())).Verifiable();

            var mockSignalRPublisher = new Mock<ISignalRPublisher>();
            mockSignalRPublisher.Setup(m => m.PublishAsync(It.IsAny<string>(), It.IsAny<SpeechProjection>())).Verifiable();

            //Act
            INotificationHandler<NotificationMessage<string>> elasticSearchNotifer = new ElasticSearchNotifer(mockEventSerializer.Object, mockJsonSerializer.Object, mockElasticSearchClient.Object, mockSignalRPublisher.Object);
            await elasticSearchNotifer.Handle(message, CancellationToken.None);

            //Assert
            mockElasticSearchClient.Verify(m => m.CreateAsync(It.IsAny<SpeechProjection>()), Times.Once);
            mockSignalRPublisher.Verify(m => m.PublishAsync(It.IsAny<string>(), It.IsAny<SpeechProjection>()), Times.Once);
        }

        [Fact]
        public async Task ElasticSearchNotiferShouldDeleteMessage()
        {
            //Arrange
            var eventStore = new EventStore(Guid.NewGuid(),
                It.IsAny<long>(), It.IsAny<string>(), "type", DateTime.Now, "body");
            var message = new NotificationMessage<string> { Message = "notifyText" };

            var @event = new SpeechDeletedEvent(Guid.NewGuid(), true);
            var mockEventSerializer = new Mock<IEventSerializer>();
            mockEventSerializer.Setup(m => m.Deserialize<Event>(It.IsAny<string>(), It.IsAny<string>())).Returns(@event);

            var mockJsonSerializer = new Mock<IJsonSerializer>();
            mockJsonSerializer.Setup(m => m.Deserialize<EventStore>(It.IsAny<string>()))
                .Returns(eventStore);

            var mockElasticSearchClient = new Mock<IElasticSearchClient<SpeechProjection>>();
            mockElasticSearchClient.Setup(m => m.DeleteAsync(It.IsAny<SpeechProjection>())).Verifiable();

            var mockSignalRPublisher = new Mock<ISignalRPublisher>();
            mockSignalRPublisher.Setup(m => m.PublishAsync(It.IsAny<string>(), It.IsAny<SpeechProjection>())).Verifiable();

            //Act
            INotificationHandler<NotificationMessage<string>> elasticSearchNotifer = new ElasticSearchNotifer(mockEventSerializer.Object, mockJsonSerializer.Object, mockElasticSearchClient.Object, mockSignalRPublisher.Object);
            await elasticSearchNotifer.Handle(message, CancellationToken.None);

            //Assert
            mockElasticSearchClient.Verify(m => m.DeleteAsync(It.IsAny<SpeechProjection>()), Times.Once);
            mockSignalRPublisher.Verify(m => m.PublishAsync(It.IsAny<string>(), It.IsAny<SpeechProjection>()), Times.Once);
        }
    }
}