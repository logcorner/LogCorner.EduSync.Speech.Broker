using Confluent.Kafka;
using LogCorner.EduSync.Speech.Command.SharedKernel;
using LogCorner.EduSync.Speech.Resiliency;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.Telemetry;
using LogCorner.EduSync.Speech.Telemetry.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services, string bootstrapServer, IConfiguration configuration)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceBus).Assembly));
            services.AddTransient<INotifierMediatorService, NotifierMediatorService>();
            services.AddSharedKernel();
            services.AddSingleton<IKafkaClusterManager, KafkaClusterManager>();
            services.AddSingleton<IKafkaClusterManager, KafkaClusterManager>();
            services.AddResiliencyServices();
            services.AddOpenTelemetryServices(configuration);

            services.AddSingleton<IServiceBusReceiver>(x =>
                {
                    var consumerConfig = new ConsumerConfig
                    {
                        BootstrapServers = bootstrapServer,
                        EnableAutoCommit = false,
                        EnableAutoOffsetStore = false,
                        MaxPollIntervalMs = 300000,
                        GroupId = "default",

                        // Read messages from start if no commit exists.
                        AutoOffsetReset = AutoOffsetReset.Earliest
                    };

                    var consumer = new ConsumerBuilder<Null, string>(consumerConfig)
                        .SetKeyDeserializer(Deserializers.Null)
                        .SetValueDeserializer(Deserializers.Utf8)
                        .SetErrorHandler((_, e) =>
                             Console.WriteLine($"Error: {e.Reason}"))
                        .Build();

                    return new KafkaReceiver(
                        consumer,
                        x.GetRequiredService<INotifierMediatorService>(), x.GetRequiredService<ITraceService>(),
                        x.GetRequiredService<ILogger<KafkaReceiver>>(), configuration);
                }
            );
        }
    }
}