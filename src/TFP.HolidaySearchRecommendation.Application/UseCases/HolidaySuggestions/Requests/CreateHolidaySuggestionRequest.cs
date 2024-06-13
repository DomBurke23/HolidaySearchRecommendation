using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.HolidaySuggestions.Requests
{
    public class CreateHolidaySuggestionRequest
    {
        public HolidaySuggestion HolidaySuggestion { get; set; }
    }
}
