using TFP.HolidaySearchRecommendation.Common.Context;

namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public class UserContextAccessor : IUserContextAccessor
    {
        public UserContext? UserContext { get; set; }
    }
}
