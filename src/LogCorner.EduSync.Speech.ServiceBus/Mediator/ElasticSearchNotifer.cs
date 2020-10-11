using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using MediatR;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using LogCorner.EduSync.Speech.Projection;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class ElasticSearchNotifer : INotificationHandler<NotificationMessage<string>>
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly IElasticSearchClient<SpeechProjection> _elasticSearchClient;

        public ElasticSearchNotifer(IEventSerializer eventSerializer,
            IElasticSearchClient<SpeechProjection> elasticSearchClient)
        {
            _eventSerializer = eventSerializer;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task Handle(NotificationMessage<string> notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Debugging from Notifier 1. Message  : {notification.Message} ");

            var eventStore = JsonSerializer.Deserialize<EventStore>(notification.Message);
            var @event = _eventSerializer.Deserialize<Event>(eventStore.TypeName,
                                                             eventStore.PayLoad);
            var projection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
            projection.Project( @event );
            await _elasticSearchClient.CreateAsync(projection);
        }
    }
}