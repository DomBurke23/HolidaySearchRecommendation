using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Common.Services;
using TFP.HolidaySearchRecommendation.Domain.Eventing.Models;
using TFP.HolidaySearchRecommendation.Domain.Services;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Services
{
    public class EventGridService : IEventService
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly EventGridOptions _eventGridOptions;
        private readonly EventGridClient _eventGridClient;

        public EventGridService(IOptions<EventGridOptions> eventGridOptions, ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
            _eventGridOptions = eventGridOptions.Value;
            _eventGridClient = new EventGridClient(new TopicCredentials(_eventGridOptions.AccessKey));
        }

        public async Task PublishEventsAsync(List<Event> events)
        {
            var eventGridEvents = new List<EventGridEvent>();

            foreach (Event evt in events)
            {
                var data = JObject.FromObject(evt.Data);
                data.Add("Tenant", _tenantContextAccessor.TenantContext.TenantOptions.Name);
                var subject = Path.Join($"/tenants/{_tenantContextAccessor.TenantContext.TenantOptions.Name}", evt.Subject);

                var eventGridEvent = new EventGridEvent
                {
                    Id = evt.Id,
                    // object not supported here, must convert to JObject - see issue https://github.com/Azure/azure-sdk-for-net/issues/12244
                    Data = data,
                    EventType = evt.Name,
                    Subject = subject,
                    EventTime = evt.Timestamp,
                    DataVersion = "1.0"
                };

                eventGridEvents.Add(eventGridEvent);
            }

            await _eventGridClient.PublishEventsAsync(new Uri(_eventGridOptions.Endpoint).Host, eventGridEvents);
        }
    }
}
