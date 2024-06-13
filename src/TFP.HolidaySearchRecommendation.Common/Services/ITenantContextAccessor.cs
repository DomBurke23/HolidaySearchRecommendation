using TFP.HolidaySearchRecommendation.Common.Context;

namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public interface ITenantContextAccessor
    {
        TenantContext? TenantContext { get; set; }
    }
}
