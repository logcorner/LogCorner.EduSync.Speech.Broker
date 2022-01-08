using Confluent.Kafka;
using LogCorner.EduSync.Speech.Command.SharedKernel.Serialyser;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Reflection;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services, string bootstrapServer)
        {
            services.AddSingleton<IServiceBus, ServiceBus>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<INotifierMediatorService, NotifierMediatorService>();

            services.AddSingleton<IServiceBusProvider>(x =>
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
                                   Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}"))
                   .SetErrorHandler((_, e) =>
                                   Console.WriteLine($"Error: {e.Reason}. Is Fatal: {e.IsFatal}"))
                   .Build();

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
                    // consumer.
                    return new KafkaClient(producer,
                        x.GetRequiredService<IJsonSerializer>(),
                        consumer,
                        x.GetRequiredService<INotifierMediatorService>()
                    );
                }
            );
        }
    }
}