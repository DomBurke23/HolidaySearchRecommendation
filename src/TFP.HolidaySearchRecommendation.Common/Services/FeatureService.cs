namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public class FeatureService : IFeatureService
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;

        public FeatureService(ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
        }

        public bool IsEnabled(string featureName)
        {
            Models.FeatureToggle? featuretoggle = _tenantContextAccessor.TenantContext.TenantOptions.FeatureToggles.FirstOrDefault(ft => ft.Name.ToUpper() == featureName.ToUpper());

            return featuretoggle != null && featuretoggle.Enabled;
        }
    }
}
