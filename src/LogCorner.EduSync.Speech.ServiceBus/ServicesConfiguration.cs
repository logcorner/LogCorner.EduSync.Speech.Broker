using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.ServiceBus
{
    public static class ServicesConfiguration
    {
        public static void AddServiceBus(this IServiceCollection services, string url)
        {
            //services.AddSingleton<IServiceBus, ServiceBus>();

            //services.AddSingleton<IJsonSerializer, CustomJsonSerializer>();

            //services.AddSingleton<IServiceBusProvider>(x =>
            //    {
            //        var producerConfig = new ProducerConfig { BootstrapServers = url };

            //        var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

            //        return new KafkaClient(producer, x.GetRequiredService<IJsonSerializer>());
            //    }
            //);
        }
    }
}