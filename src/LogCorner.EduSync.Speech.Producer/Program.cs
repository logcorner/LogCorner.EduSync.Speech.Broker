using LogCorner.EduSync.SignalR.Common;
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
                    services.AddSingleton<ISignalRNotifier, SignalRNotifier>();
                    services.AddSingleton<ISignalRPublisher, SignalRPublisher>();

                    services.AddSingleton<IHubConnectionInstance, HubConnectionInstance>(ctx =>
                    {
                        var hubConnectionInstance = new HubConnectionInstance();
                        hubConnectionInstance.ConnectAsync().Wait();

                        return hubConnectionInstance;
                    });
                });
    }
}