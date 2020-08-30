using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IKafkaClient, KafkaClient>(ctx =>
            {
                string url = "localhost:9092";

                var producerConfig = new ProducerConfig { BootstrapServers = url };

                var producer = new ProducerBuilder<Null, string>(producerConfig).Build();
                return new KafkaClient(producer, new CustomJsonSerializer());
            });

            services.AddSingleton<IServiceBus, ServiceBus>();
        }
    }
}