using Confluent.Kafka;
using LogCorner.EduSync.Speech.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public class KafkaProducer : IServiceBusProducer
    {
        private readonly IProducer<Null, string> _producer;

        private readonly ITraceService _traceService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KafkaProducer> _logger;

        public KafkaProducer(IProducer<Null, string> producer, ITraceService traceService, IConfiguration configuration, ILogger<KafkaProducer> logger)
        {
            _producer = producer;

            _traceService = traceService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendAsync(string topic, string @event)
        {
            using var activity = StartActivity(topic, @event, out var kafkaHeaders);

            var t = _producer.ProduceAsync(topic, new Message<Null, string>
            { Value = @event, Headers = kafkaHeaders });

            _logger.LogInformation($"**KafkaProducer::SendAsync - jsonString = {@event}");
            await t.ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    _logger.LogInformation($"**KafkaProducer::SendAsync - error = {task.Exception?.Message}");
                }
                else
                {
                    _logger.LogInformation($"**KafkaProducer::SendAsync - produced : {@event}");

                    _logger.LogInformation($"**KafkaProducer::SendAsync - wrote to offset: {task.Result?.Offset}");
                }
            });
        }

        private Activity? StartActivity(string topic, string @event, out Headers kafkaHeaders)
        {
            kafkaHeaders = new Headers();
            string sourceName = _configuration["OpenTelemetry:SourceName"];

            if (sourceName == null)
            {
                _logger.LogInformation("**KafkaProducer::StartActivity: OpenTelemetry SourceName is not not configured in appSetting");
                return Activity.Current;
            }
            ActivitySource activitySource = new(sourceName);
            TextMapPropagator propagatorProducer = Propagators.DefaultTextMapPropagator;
            using var activity =
                activitySource.StartActivity($"Publishing event to Kafka topic {topic}", ActivityKind.Producer);

            IDictionary<string, byte[]> props = new Dictionary<string, byte[]>();
            _traceService.AddActivityToHeader(activity, props, propagatorProducer);
            foreach (var item in props)
            {
                kafkaHeaders.Add(item.Key, item.Value);
            }

            var tags = new Dictionary<string, object>
            {
                { "@event", @event }
            };
            _traceService.SetActivityTags(activity, tags);
            return activity;
        }
    }
}