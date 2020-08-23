using LogCorner.EduSync.SignalR.Common;
using LogCorner.EduSync.SignalR.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Speech.Producer.UnitTests
{
    public class HubConnectionInstanceMock : IHubConnectionInstance
    {
        public HubConnection Connection { get; private set; }

        public async Task ConnectAsync()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSignalR();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
                    });
                });

            var server = new TestServer(webHostBuilder);
            Connection = new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost/logcornerhub",
                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
                .Build();
        }
    }
}