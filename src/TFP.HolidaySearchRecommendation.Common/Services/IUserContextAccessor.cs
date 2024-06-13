using TFP.HolidaySearchRecommendation.Common.Context;

namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public interface IUserContextAccessor
    {
        UserContext? UserContext { get; set; }
    }
}
