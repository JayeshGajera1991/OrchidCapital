using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchid.EmailService.Repository
{
    public class DynamicsAuthService
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _orgUrl;

        public DynamicsAuthService(string tenantId, string clientId, string clientSecret, string orgUrl)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _orgUrl = orgUrl;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var app = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority($"https://login.microsoftonline.com/{_tenantId}")
                .Build();

            var result = await app
                .AcquireTokenForClient(new[] { $"{_orgUrl}/.default" })
                .ExecuteAsync();

            return result.AccessToken;
        }
    }
}
