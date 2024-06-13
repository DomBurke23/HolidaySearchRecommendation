using TFP.HolidaySearchRecommendation.Domain.Models;

namespace TFP.HolidaySearchRecommendation.Domain.Data.Repositories
{
    public interface IHolidaySuggestionRepository
    {
        Task<HolidaySuggestion> FindAsync(string reference);
        Task SaveAsync(HolidaySuggestion holidaySuggestion);
    }
}
