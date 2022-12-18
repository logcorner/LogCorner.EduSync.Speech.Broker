using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.Telemetry;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace LogCorner.EduSync.Speech.Producer
{
    public static class ServicesConfiguration
    {
        public static void AddProducer(this IServiceCollection services, string bootstrapServer, IConfiguration configuration)
        {
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddSingleton<IServiceBusProducer>(x =>
             {
                 var producerConfig = new ProducerConfig
                 {
                     BootstrapServers = bootstrapServer,
                     EnableDeliveryReports = true,
                     ClientId = Dns.GetHostName(),
                     Debug = "msg",

                     // retry settings:
                     // Receive acknowledgement from all sync replicas
                     Acks = Acks.All,
                     // Number of times to retry before giving up
                     MessageSendMaxRetries = 3,
                     // Duration to retry before next attempt
                     RetryBackoffMs = 1000,
                     // Set to true if you don't want to reorder messages on retry
                     EnableIdempotence = true
                 };
                 var producer = new ProducerBuilder<Null, string>(producerConfig)
                     .SetKeySerializer(Serializers.Null)
                     .SetValueSerializer(Serializers.Utf8)
                     .SetLogHandler((_, message) =>
                          Console.WriteLine(
                             $"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                     .SetErrorHandler((_, e) =>
                          Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                     .Build();

                 return new KafkaProducer(producer,
                     // x.GetRequiredService<IJsonSerializer>(),
                     x.GetRequiredService<ITraceService>(), configuration, x.GetRequiredService<ILogger<KafkaProducer>>());
             }
             );
        }
    }
}