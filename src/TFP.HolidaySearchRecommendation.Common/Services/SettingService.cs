using TFP.HolidaySearchRecommendation.Common.Exceptions;
using TFP.HolidaySearchRecommendation.Common.Models;

namespace TFP.HolidaySearchRecommendation.Common.Services
{
    public class SettingService : ISettingService
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;

        public SettingService(ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
        }

        public string Get(string settingName)
        {
            Setting setting = _tenantContextAccessor.TenantContext.TenantOptions.Settings.FirstOrDefault(s => s.Name.ToUpper() == settingName.ToUpper());

            if (setting == null)
            {
                throw new SettingNotFoundException(settingName);
            }

            return setting.Value;
        }
    }
}
