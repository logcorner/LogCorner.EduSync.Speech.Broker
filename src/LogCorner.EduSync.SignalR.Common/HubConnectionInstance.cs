using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubConnectionInstance
    {
        public HubConnection Connection { get; private set; }

        public async Task ConnectAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:5001/logcornerhub")
                .Build();

            await Connection.StartAsync();
        }
    }
}