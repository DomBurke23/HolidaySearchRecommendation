using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests
{
    public class EnqueueCreateHolidaySuggestionRequest
    {
        public string OperationReference { get; set; }
        public string OperationRequester { get; set; }
        public HolidaySuggestion HolidaySuggestion { get; set; }
    }
}
