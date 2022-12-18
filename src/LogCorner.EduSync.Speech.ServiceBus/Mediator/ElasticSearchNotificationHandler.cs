using LogCorner.EduSync.Notification.Common.Hub;
using LogCorner.EduSync.Speech.Command.SharedKernel.Events;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.Resiliency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LogCorner.EduSync.Speech.ServiceBus.Mediator;

public class ElasticSearchNotificationHandler : INotificationHandler<NotificationMessage<string>>
{
    private readonly IEventSerializer _eventSerializer;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IElasticSearchClient<SpeechProjection> _elasticSearchClient;
    private readonly ILogger<ElasticSearchNotificationHandler> _logger;
    private readonly ISignalRPublisher _publisher;
    private readonly IResiliencyService _resiliencyService;

    public ElasticSearchNotificationHandler(IEventSerializer eventSerializer, IJsonSerializer jsonSerializer, IElasticSearchClient<SpeechProjection> elasticSearchClient,
        ILogger<ElasticSearchNotificationHandler> logger, IResiliencyService resiliencyService, ISignalRPublisher publisher)
    {
        _eventSerializer = eventSerializer;
        _jsonSerializer = jsonSerializer;
        _elasticSearchClient = elasticSearchClient;
        _logger = logger;
        _resiliencyService = resiliencyService;
        _publisher = publisher;
    }

    public async Task Handle(NotificationMessage<string> notificationMessage, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(notificationMessage);
        var eventStore = _jsonSerializer.Deserialize<EventStore>(notificationMessage.Message);
        var @event = _eventSerializer.DeserializeEvent<Event>(eventStore.PayLoad, eventStore.TypeName);
        var projection = Invoker.CreateInstanceOfProjection<SpeechProjection>();
        projection.Project(@event);
        await _elasticSearchClient.ProjectEventAsync(projection, projection.IsDeleted).ContinueWith(
            result =>
            {
                if (result.Status == TaskStatus.RanToCompletion)
                {
                    _resiliencyService.ExponentialExceptionRetry.ExecuteAsync(async () => await _publisher.PublishAsync("ReadModelAcknowledged", null, projection));
                    _logger.LogInformation($"**ElasticSearchNotificationHandler::Handle - ProjectEventAsync {projection.Id} ");
                }
                else if (result.Status == TaskStatus.Faulted)
                {
                    _logger.LogInformation($"**ElasticSearchNotificationHandler::Handle - ProjectEventAsync {result.Exception?.GetBaseException().Message}");
                }
            }, cancellationToken);
    }
}