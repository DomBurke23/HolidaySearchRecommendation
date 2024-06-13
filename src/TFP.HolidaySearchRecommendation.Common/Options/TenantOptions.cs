using TFP.HolidaySearchRecommendation.Common.Models;

namespace TFP.HolidaySearchRecommendation.Common.Options
{
    public class TenantOptions
    {
        public string Name { get; set; }
        public List<Setting> Settings { get; set; }
        public List<FeatureToggle> FeatureToggles { get; set; }
    }
}
