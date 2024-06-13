using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;

namespace TFP.HolidaySearchRecommendation.Application.Middleware
{
    public interface IServiceBusMiddleware
    {
        Task<Message> InvokeAsync<T>(JObject content, string messageId, string subject, IDictionary<string, object> applicationProperties);
    }
}
