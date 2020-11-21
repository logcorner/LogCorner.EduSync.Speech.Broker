using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class ElasticSearchNotifer : INotificationHandler<NotificationMessage<string>>
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IElasticSearchClient<SpeechProjection> _elasticSearchClient;
        private readonly ISignalRPublisher _publisher;

        public ElasticSearchNotifer(IEventSerializer eventSerializer, IJsonSerializer jsonSerializer,
            IElasticSearchClient<SpeechProjection> elasticSearchClient, ISignalRPublisher publisher)
        {
            _eventSerializer = eventSerializer;
            _jsonSerializer = jsonSerializer;
            _elasticSearchClient = elasticSearchClient;
            _publisher = publisher;
        }

        public async Task Handle(NotificationMessage<string> notificationMessage, CancellationToken cancellationToken)
        {
            if (notificationMessage == null)
            {
                throw new ArgumentNullException(nameof(notificationMessage));
            }
            var eventStore = _jsonSerializer.Deserialize<EventStore>(notificationMessage.Message);
            var @event = _eventSerializer.Deserialize<Event>(eventStore.TypeName, eventStore.PayLoad);
            var projection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            projection.Project(@event);
            if (projection.IsDeleted)
            {
                await _elasticSearchClient.DeleteAsync(projection);
            }
            else
            {
                await _elasticSearchClient.CreateAsync(projection);
            }
            await _publisher.PublishAsync(Topics.Speech, projection);
        }
    }
}