using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using MediatR;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class ElasticSearchNotifer : INotificationHandler<NotificationMessage<string>>
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly IElasticSearchClient<SpeechView> _elasticSearchClient;

        public ElasticSearchNotifer(IEventSerializer eventSerializer,
            IElasticSearchClient<SpeechView> elasticSearchClient)
        {
            _eventSerializer = eventSerializer;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task Handle(NotificationMessage<string> notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Debugging from Notifier 1. Message  : {notification.Message} ");

            var eventStore = JsonSerializer.Deserialize<EventStore>(notification.Message);
            var entity = _eventSerializer.Deserialize<Event>(eventStore.TypeName, eventStore.PayLoad);
            var view = Invoker.CreateInstanceOfAggregateRoot<SpeechView>();
            view.LoadFromHistory(new IDomainEvent[] { entity });
            await _elasticSearchClient.CreateAsync(view);
        }
    }
}