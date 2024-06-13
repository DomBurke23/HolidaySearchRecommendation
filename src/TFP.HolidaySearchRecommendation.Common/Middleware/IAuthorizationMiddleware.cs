using Microsoft.AspNetCore.Http;

namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    public interface IAuthorizationMiddleware
    {
        Task InvokeAsync(HttpRequest httpRequest);
    }
}
