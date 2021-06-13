using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubInstance
    {
        private readonly IConfiguration _configuration;
        private string Url { get; }
        public HubConnection Connection { get; private set; }

        public HubConnectionInstance(string url, IConfiguration configuration)
        {
            _configuration = configuration;
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
            string clientId = _configuration["AzureAd:ClientId"];
            string clientSecret = _configuration["AzureAd:ClientSecret"];
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            Connection = new HubConnectionBuilder()
                .WithUrl(Url, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(result.AccessToken);
                })
                .Build();
        }

        public async Task StartAsync()
        {
            await Connection.StartAsync();
        }
    }
}