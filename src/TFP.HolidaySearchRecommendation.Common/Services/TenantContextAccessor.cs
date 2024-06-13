using TFP.HolidaySearchRecommendation.Common.Context;

namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public class TenantContextAccessor : ITenantContextAccessor
    {
        public TenantContext? TenantContext { get; set; }
    }
}
