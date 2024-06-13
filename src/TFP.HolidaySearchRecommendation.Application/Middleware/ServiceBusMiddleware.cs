using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Application.Exceptions;
using TFP.HolidaySearchRecommendation.Common.Constants;
using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;

namespace TFP.HolidaySearchRecommendation.Application.Middleware
{
    public class ServiceBusMiddleware : IServiceBusMiddleware
    {
        public async Task<Message> InvokeAsync<T>(JObject content, string messageId, string subject, IDictionary<string, object> applicationProperties)
        {
            applicationProperties.TryGetValue(CustomHeaderConstants.Tenant, out var _tenant);
            var tenant = (string)_tenant;

            if (string.IsNullOrWhiteSpace(tenant))
            {
                throw new BadMessageException($"Tenant not provided");
            }

            if (content == null)
            {
                throw new BadMessageException($"Content not provided");
            }

            // TODO : Validate message attributes with fluent validation
            var message = new Message()
            {
                Id = messageId,
                Subject = subject,
                Tenant = tenant,
                Data = content.ToObject<T>()
            };

            return message;
        }
    }
}
