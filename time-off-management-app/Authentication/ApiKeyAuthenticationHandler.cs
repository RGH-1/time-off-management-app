using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using time_off_management_app.Services;

namespace time_off_management_app.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApiKeyService _apiKeyService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ApiKeyService apiKeyService)
            : base(options, logger, encoder)
        {
            _apiKeyService = apiKeyService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.TryGetValue("Company-API-Key", out var apiKeyHeader))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = apiKeyHeader.ToString();

            if(String.IsNullOrWhiteSpace(apiKey))
            {
                return AuthenticateResult.Fail("No API Key");
            }

            var apiKeyRecord = await _apiKeyService.ValidateKeyAsync(apiKey);

            if(apiKeyRecord == null)
            {
                return AuthenticateResult.Fail("Invalid Key");
            }

            var claims = new List<Claim> 
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    apiKeyRecord.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    apiKeyRecord.Name),

                new Claim(
                    "auth_method",
                    "api_key")
            };

            var identity = new ClaimsIdentity(
                claims,
                Scheme.Name);

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(
                principal,
                Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
