using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator
{
    public class ElasticSearchNotifer : INotificationHandler<NotificationMessage<string>>
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IElasticSearchClient<SpeechProjection> _elasticSearchClient;

        public ElasticSearchNotifer(IEventSerializer eventSerializer, IJsonSerializer jsonSerializer,
            IElasticSearchClient<SpeechProjection> elasticSearchClient)
        {
            _eventSerializer = eventSerializer;
            _jsonSerializer = jsonSerializer;
            _elasticSearchClient = elasticSearchClient;
        }

        public async Task Handle(NotificationMessage<string> notification, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Debugging from Notifier 1. Message  : {notification.Message} ");

            try
            {
                var eventStore = _jsonSerializer.Deserialize<EventStore>(notification.Message);
                var @event = _eventSerializer.Deserialize<Event>(eventStore.TypeName,eventStore.PayLoad);
                var projection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
                projection.Project(@event);
                await _elasticSearchClient.CreateAsync(projection);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}