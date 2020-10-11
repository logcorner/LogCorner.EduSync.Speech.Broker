using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.SignalR.Common
{
    public static class ServicesConfiguration
    {
        public static void AddSignalRServices(this IServiceCollection services)
        {
            services.AddSingleton<ISignalRNotifier, SignalRNotifier>();
            services.AddSingleton<ISignalRPublisher, SignalRPublisher>();

            services.AddSingleton<IHubInstance, HubConnectionInstance>(ctx =>
            {
                var hubConnectionInstance = new HubConnectionInstance("https://localhost:5001/logcornerhub");
                hubConnectionInstance.InitAsync().Wait();
                return hubConnectionInstance;
            });
        }
    }
}