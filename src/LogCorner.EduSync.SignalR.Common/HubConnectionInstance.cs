using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubInstance
    {
        private readonly IIdentityProvider _identityProvider;
        private string Url { get; }
        public HubConnection Connection { get; private set; }

        public HubConnectionInstance(string url, IIdentityProvider identityProvider)
        {
            _identityProvider = identityProvider;
            Url = url;
        }

        public async Task InitAsync()
        {
            Connection = new HubConnectionBuilder()
                .WithUrl(Url)
                .Build();

            await Task.CompletedTask;
        }

        public async Task InitConfidentialClientAsync()
        {
            var scopes = new[] { "https://datasynchrob2c.onmicrosoft.com/signalr/hub/.default" };
          
            var accessToken = await _identityProvider.AcquireTokenForClient(scopes);
            Connection = new HubConnectionBuilder()
                .WithUrl(Url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .Build();
        }

        public async Task StartAsync()
        {
            await Connection.StartAsync();
        }
    }
}