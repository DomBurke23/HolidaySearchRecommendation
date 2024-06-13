using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;

namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    public interface ITenantMiddleware
    {
        Task InvokeAsync(string tenant);
        Task InvokeAsync(HttpContext context);
        Task InvokeAsync(IDictionary<string, object> applicationProperties);
        Task InvokeAsync(EventGridEvent eventGridEvent);
    }
}
