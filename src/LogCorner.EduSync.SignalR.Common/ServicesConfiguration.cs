using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.SignalR.Common
{
    public static class ServicesConfiguration
    {
        public static void AddSignalRServices(this IServiceCollection services, string endpoint, IConfiguration configuration)
        {
            services.AddSingleton<ISignalRNotifier, SignalRNotifier>();
            services.AddSingleton<ISignalRPublisher, SignalRPublisher>();

            services.AddSingleton<IHubInstance, HubConnectionInstance>(ctx =>
            {
                var hubConnectionInstance = new HubConnectionInstance(endpoint, new IdentityProvider(configuration));
                hubConnectionInstance.InitConfidentialClientAsync().Wait();
                return hubConnectionInstance;
            });
        }
    }
}