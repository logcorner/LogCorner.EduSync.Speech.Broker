using Microsoft.Extensions.Configuration;
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
            //TODO : uncomment when client credential is enabled on Azure AD B2C
            /*string clientId = _configuration["AzureAd:ClientId"];
            string clientSecret = _configuration["AzureAd:ClientSecret"];
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs)
                .Build();

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;*/
            return await Task.FromResult("this is a fake access token");
        }
    }
}