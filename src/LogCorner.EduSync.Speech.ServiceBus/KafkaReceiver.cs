using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace LogCorner.EduSync.Speech.ServiceBus;

public class KafkaReceiver : IServiceBusReceiver
{
    private readonly IConsumer<Null, string> _consumer;

    private readonly INotifierMediatorService _notifierMediatorService;
    private readonly ITraceService _traceService;
    private readonly ILogger<KafkaReceiver> _logger;
    private readonly IConfiguration _configuration;

    public KafkaReceiver(IConsumer<Null, string> consumer, INotifierMediatorService notifierMediatorService, ITraceService traceService,
        ILogger<KafkaReceiver> logger, IConfiguration configuration)
    {
        _consumer = consumer;
        _notifierMediatorService = notifierMediatorService;
        _traceService = traceService;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task ReceiveAsync(string[] topics, CancellationToken stoppingToken, bool runAlways = true)
    {
        try
        {
            _consumer.Subscribe(topics);
            _logger.LogInformation($"**KafkaClient::ReceiveAsync - consuming on topic {string.Join(' ', topics)}");
            while (true)
            {
                var data = _consumer.Consume();
                IDictionary<string, byte[]> props = new Dictionary<string, byte[]>();
                foreach (var item in data.Message.Headers)
                {
                    var bytes = item.GetValueBytes();
                    if (bytes != null)
                    {
                        props.TryAdd(item.Key, bytes);
                    }
                }
                string sourceName = _configuration["OpenTelemetry:SourceName"];
                ActivitySource activitySource = new(sourceName);
                TextMapPropagator propagator = new TraceContextPropagator();
                var parentContext = propagator.Extract(default, props, _traceService.ExtractTraceContextFromBasicProperties);
                Baggage.Current = parentContext.Baggage;

                using var activity = activitySource.StartActivity("Process Message from kafka", ActivityKind.Consumer,
                    parentContext.ActivityContext);
                if (data.Message?.Value == null)
                {
                    continue;
                }
                var tags = new Dictionary<string, object>
                {
                    {"key", data.Message.Key },
                    {"partition", data.Partition.Value},
                    {"topic",data.Topic} ,
                    {"payload", data.Message.Value}
                };
                _traceService.SetActivityTags(activity, tags);
                _logger.LogInformation($"**KafkaClient::ReceiveAsync - key : {data.Message.Key}");

                _logger.LogInformation($"**KafkaClient::ReceiveAsync - partition : {data.Partition.Value}");
                _logger.LogInformation($"**KafkaClient::ReceiveAsync - offset : {data.Offset.Value}");
                var message = new NotificationMessage<string> { Message = data.Message.Value };
                _logger.LogInformation($"**KafkaClient::ReceiveAsync - message : {message.Message}");
                await _notifierMediatorService.Notify(message);

                _consumer.Commit(data);
                _consumer.StoreOffset(data);
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
        catch (KafkaException e)
        {
            _logger.LogInformation($"Consume error: {e.Message}");
            _logger.LogInformation("Exiting producer...");
        }
        finally
        {
            _consumer.Close();
        }
    }
}