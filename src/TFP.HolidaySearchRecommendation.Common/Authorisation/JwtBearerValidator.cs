using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using TFP.HolidaySearchRecommendation.Common.Options;

namespace TFP.HolidaySearchRecommendation.Common.Authorisation
{
    public class JwtBearerValidator : IJwtBearerValidator
    {
        private readonly IOptions<JwtBearerOptions> _jwtBearerOptions;
        private readonly ILogger _log;
        private const string _scopeType = @"http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

        private ConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
        private ClaimsPrincipal _claimsPrincipal;

        private readonly string _wellKnownEndpoint = string.Empty;
        private readonly string _requiredScope = "access_as_application";

        public JwtBearerValidator(IOptions<JwtBearerOptions> jwtBearerOptions, ILoggerFactory loggerFactory)
        {
            _log = loggerFactory.CreateLogger<JwtBearerValidator>();
            _jwtBearerOptions = jwtBearerOptions;
            _wellKnownEndpoint = $"{_jwtBearerOptions.Value.Authority}/v2.0/.well-known/openid-configuration";
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(HttpRequest httpRequest)
        {
            string authorizationHeader = httpRequest.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return null;
            }

            if (!authorizationHeader.Contains("Bearer"))
            {
                return null;
            }

            var accessToken = authorizationHeader.Substring("Bearer ".Length);

            OpenIdConnectConfiguration oidcWellknownEndpoints = await GetOIDCWellknownConfiguration();

            var tokenValidator = new JwtSecurityTokenHandler();


            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidAudience = _jwtBearerOptions.Value.Audience,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = _jwtBearerOptions.Value.ValidateLifetime,
                ValidateTokenReplay = true,
                IssuerSigningKeys = oidcWellknownEndpoints.SigningKeys,
                ValidIssuer = _jwtBearerOptions.Value.Issuer
            };

            try
            {
                // Uncomment below if need to check Issuer on response
                // IdentityModelEventSource.ShowPII = true; 
                _claimsPrincipal = tokenValidator.ValidateToken(accessToken, validationParameters, out SecurityToken securityToken);

                if (IsScopeValid(_requiredScope))
                {
                    return _claimsPrincipal;
                }

                return null;
            }
            catch (Exception ex)
            {
                _log.LogError(ex.ToString());
            }
            return null;
        }

        private async Task<OpenIdConnectConfiguration> GetOIDCWellknownConfiguration()
        {
            _log.LogDebug($"Get OIDC well known endpoints {_wellKnownEndpoint}");
            _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                 _wellKnownEndpoint, new OpenIdConnectConfigurationRetriever());

            return await _configurationManager.GetConfigurationAsync();
        }

        private bool IsScopeValid(string scopeName)
        {
            if (_claimsPrincipal == null)
            {
                _log.LogWarning($"Scope invalid {scopeName}");
                return false;
            }

            var scopeClaim = _claimsPrincipal.HasClaim(x => x.Type == _scopeType)
                ? _claimsPrincipal.Claims.First(x => x.Type == _scopeType).Value
                : string.Empty;

            if (string.IsNullOrEmpty(scopeClaim))
            {
                _log.LogWarning($"Scope invalid {scopeName}");
                return false;
            }

            if (!scopeClaim.Equals(scopeName, StringComparison.OrdinalIgnoreCase))
            {
                _log.LogWarning($"Scope invalid {scopeName}");
                return false;
            }

            _log.LogDebug($"Scope valid {scopeName}");
            return true;
        }
    }
}
