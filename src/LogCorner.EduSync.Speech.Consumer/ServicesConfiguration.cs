using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Speech.Consumer
{
    public static class ServicesConfiguration
    {
        public static void AddConsumer(this IServiceCollection services, IConfiguration configuration, string kafkaEndpoint, string elasticeSearhEndpoint, string elasticeSearhIndex)
        {
            services.AddServiceBus(kafkaEndpoint, configuration);
            services.AddElasticSearch<SpeechProjection>(elasticeSearhEndpoint, elasticeSearhIndex);
            services.AddSingleton<IConsumerService, ConsumerService>();
        }
    }
}