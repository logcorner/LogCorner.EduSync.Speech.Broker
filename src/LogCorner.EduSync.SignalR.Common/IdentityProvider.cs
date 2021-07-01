using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class IdentityProvider : IIdentityProvider
    {
        private IConfiguration _configuration;

        public IdentityProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AcquireTokenForClient(string[] scopes)
        {
            string clientId = _configuration["AzureAd:ClientId"];
            string clientSecret = _configuration["AzureAd:ClientSecret"];
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }
    }
}