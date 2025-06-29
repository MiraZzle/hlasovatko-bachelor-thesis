using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using server.Models.Auth;
using server.Services;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace server.Auth
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        private const string ApiKeyHeaderName = "X-Api-Key";
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyAuthHandler(
            IOptionsMonitor<ApiKeyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IApiKeyService apiKeyService) : base(options, logger, encoder) {
            _apiKeyService = apiKeyService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            // Check if request has the API key header
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues)) {
                return AuthenticateResult.NoResult();
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(providedApiKey)) {
                return AuthenticateResult.NoResult();
            }

            var user = await _apiKeyService.GetUserFromApiKeyAsync(providedApiKey);

            // Create user claims
            if (user != null) {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid API Key");
        }
    }
}
