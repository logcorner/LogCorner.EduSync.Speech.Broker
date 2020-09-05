using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services, string url)
        {
            services.AddSingleton<IChannelService, ChannelService>();
            services.AddSingleton<IKafkaClient, KafkaClient>(ctx =>
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

                return new KafkaClient(producer, new CustomJsonSerializer(), consumer, new ChannelService());
            });

            services.AddSingleton<IServiceBus, ServiceBus>();
        }
    }
}