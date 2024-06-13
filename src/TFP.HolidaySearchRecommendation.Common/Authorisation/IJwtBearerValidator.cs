using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TFP.HolidaySearchRecommendation.Common.Authorisation
{
    public interface IJwtBearerValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(HttpRequest httpRequest);
    }
}
