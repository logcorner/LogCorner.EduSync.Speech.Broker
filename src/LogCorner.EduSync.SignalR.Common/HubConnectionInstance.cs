using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubConnectionInstance
    {
       
        public HubConnection Connection { get; set; }

        public async Task ConnectAsync()
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost/echo")
                .Build();

            connection.On<string>("OnMessageRecieved", msg =>
            {
               
            });

            await connection.StartAsync();
           
        }
    }
}