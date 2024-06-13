using TFP.HolidaySearchRecommendation.Domain.Messaging.Models;

namespace TFP.HolidaySearchRecommendation.Domain.Services
{
    public interface IMessageService
    {
        Task SendAsync(Message message, string topic);
        Task SendScheduledAsync(Message message, string topic, DateTime scheduledDateTime);
    }
}
