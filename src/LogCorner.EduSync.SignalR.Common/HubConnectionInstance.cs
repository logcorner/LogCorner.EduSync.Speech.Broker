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
            Url = url;
            _identityProvider = identityProvider;
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

        public async Task InitConfidentialClientAsync()
        {
            var scopes = new[] { "https://datasynchrob2c.onmicrosoft.com/signalr/hub/.default" };

            var AccessToken = await _identityProvider.AcquireTokenForClient(scopes);
            Connection = new HubConnectionBuilder()
                .WithUrl(Url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AccessToken);
                })
                .Build();
        }
    }
}