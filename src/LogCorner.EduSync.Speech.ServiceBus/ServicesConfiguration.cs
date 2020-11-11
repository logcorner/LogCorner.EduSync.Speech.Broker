using System.Reflection;
using Confluent.Kafka;
using LogCorner.EduSync.Speech.ServiceBus.Mediator;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services, string url)
        {

            services.AddSingleton<IServiceBus, ServiceBus>();

           // services.AddSingleton<IJsonSerializer, CustomJsonSerializer>();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<INotifierMediatorService, NotifierMediatorService>();

            services.AddSingleton<IServiceBusProvider>(x =>
                {
                    var producerConfig = new ProducerConfig { BootstrapServers = url };

                    var consumerConfig = new ConsumerConfig
                    {
                        GroupId = "gid-consumers",
                        BootstrapServers = url,
                        AutoOffsetReset = AutoOffsetReset.Earliest
                    };
                    var consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
                    var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

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