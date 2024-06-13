using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Domain.Messaging.Content
{
    public class CreateHolidaySuggestionMessageContent
    {
        public string OperationReference { get; set; }
        public string OperationRequester { get; set; }
        public HolidaySuggestion HolidaySuggestion { get; set; }
    }
}
