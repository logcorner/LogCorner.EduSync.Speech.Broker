//using LogCorner.EduSync.SignalR.Common;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.SignalR.Client;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.DependencyInjection;
//using System.Threading.Tasks;

//namespace LogCorner.EduSync.Speech.Producer.UnitTests
//{
//    public class HubConnectionInstanceMock : IHubInstance
//    {
//        public HubConnection Connection { get; private set; }

//        public async Task InitAsync()
//        {
//            var webHostBuilder = new WebHostBuilder()
//                .ConfigureServices(services =>
//                {
//                    services.AddSignalR();
//                })
//                .Configure(app =>
//                {
//                    app.UseRouting();
//                    app.UseEndpoints(endpoints =>
//                    {
//                        endpoints.MapHub<LogCornerHub<object>>("/logcornerhub");
//                    });
//                });

//            var server = new TestServer(webHostBuilder);
//            Connection = new HubConnectionBuilder()
//                .WithUrl(
//                    "http://localhost/logcornerhub",
//                    o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
//                .Build();

//            await Task.CompletedTask;
//        }

//        public Task InitConfidentialClientAsync()
//        {
//            throw new System.NotImplementedException();
//        }

//        public Task StartAsync()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}