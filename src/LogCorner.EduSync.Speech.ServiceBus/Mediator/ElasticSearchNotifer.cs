using LogCorner.EduSync.Notification.Common.Hub;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
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
                await _elasticSearchClient.DeleteAsync(projection).ContinueWith(
                        result =>
                        {
                            if (result.Status == TaskStatus.RanToCompletion)
                            {
                                Console.WriteLine($"**ElasticSearchNotifer::Handle - DeleteAsync {projection.Id} ");
                                _publisher.PublishAsync(Topics.ReadModelAcknowledged, projection);
                            }
                            else if (result.Status == TaskStatus.Faulted)
                            {
                                Console.WriteLine($"**ElasticSearchNotifer::Handle - DeleteAsync {result.Exception?.GetBaseException().Message}");
                            }
                        }, cancellationToken);
            }
            else
            {
                await _elasticSearchClient.CreateAsync(projection).ContinueWith(
                    result =>
                    {
                        if (result.Status == TaskStatus.RanToCompletion)
                        {
                            Console.WriteLine($"**ElasticSearchNotifer::Handle - CreateAsync {projection.Id} ");
                            _publisher.PublishAsync(Topics.ReadModelAcknowledged, projection);
                        }
                        else if (result.Status == TaskStatus.Faulted)
                        {
                            Console.WriteLine($"**ElasticSearchNotifer::Handle - CreateAsync {result.Exception?.GetBaseException().Message}");
                        }
                    }, cancellationToken);
            }
        }
    }
}