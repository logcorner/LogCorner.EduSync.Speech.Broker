using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.ReadModel.SpeechAggregate;
using LogCorner.EduSync.Speech.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogCorner.EduSync.Speech.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddServiceBus("localhost:9092");
                    services.AddSingleton<IConsumerService, ConsumerService>();
                    services.AddHostedService<ConsumerHostedService>();

                    services.AddSharedKernel();

                    services.AddElasticSearch<SpeechView>("http://localhost:9200", "speech");
                });
    }
}