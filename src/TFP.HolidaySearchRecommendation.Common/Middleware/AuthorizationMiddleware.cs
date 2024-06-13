using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TFP.HolidaySearchRecommendation.Common.Authorisation;
using TFP.HolidaySearchRecommendation.Common.Constants;
using TFP.HolidaySearchRecommendation.Common.Context;
using TFP.HolidaySearchRecommendation.Common.Models;
using TFP.HolidaySearchRecommendation.Common.Services;

namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    public class AuthorizationMiddleware : IAuthorizationMiddleware
    {
        private readonly IJwtBearerValidator _jwtBearerValidator;
        private readonly IUserContextAccessor _userContextAccessor;

        public AuthorizationMiddleware(IJwtBearerValidator jwtBearerValidator,
            IUserContextAccessor userContextAccessor)
        {
            _jwtBearerValidator = jwtBearerValidator;
            _userContextAccessor = userContextAccessor;
        }

        public async Task InvokeAsync(HttpRequest httpRequest)
        {
            ClaimsPrincipal principal = await _jwtBearerValidator.ValidateTokenAsync(httpRequest);

            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Failed to validate Bearer token");
            }

            Claim objectIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimConstants.ObjectId);
            // forbid when claim is not found
            if (objectIdClaim == null || objectIdClaim.Value == null)
            {
                throw new UnauthorizedAccessException($"Claim {ClaimConstants.ObjectId} not found");
            }
            var userReference = objectIdClaim.Value;

            var user = new User()
            {
                UserReference = userReference
            };

            _userContextAccessor.UserContext = new UserContext()
            {
                User = user
            };
        }
    }
}
