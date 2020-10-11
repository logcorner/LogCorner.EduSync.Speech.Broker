using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubInstance
    {
        private string Url { get; }
        public HubConnection Connection { get; private set; }

        public HubConnectionInstance(string url)
        {
            Url = url;
        }

        public async Task InitAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(Url)
                .Build();

            await Task.CompletedTask;
        }

        public async Task StartAsync()
        {
            await Connection.StartAsync();
        }
    }
}