using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.Speech.ElasticSearch;
using LogCorner.EduSync.Speech.Projection;
using LogCorner.EduSync.Speech.ServiceBus;
using LogCorner.EduSync.Speech.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogCorner.EduSync.Speech.Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IProducerService, ProducerService>();
                    services.AddHostedService<ProducerHostedService>();
                    services.AddSignalRServices("http://localhost:5000/logcornerhub");
                    services.AddElasticSearch<SpeechProjection>(null, null);
                    services.AddSharedKernel();

                    services.AddServiceBus("localhost:9092");
                });
    }
}