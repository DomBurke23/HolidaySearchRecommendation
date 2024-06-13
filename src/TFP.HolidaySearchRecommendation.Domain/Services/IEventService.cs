using TFP.HolidaySearchRecommendation.Domain.Eventing.Models;

namespace TFP.HolidaySearchRecommendation.Domain.Services
{
    public interface IEventService
    {
        Task PublishEventsAsync(List<Event> events);
    }
}
