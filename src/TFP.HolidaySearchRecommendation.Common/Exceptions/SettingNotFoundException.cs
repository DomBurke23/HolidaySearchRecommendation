namespace TFP.HolidaySearchRecommendation.Common.Exceptions
{
    public class SettingNotFoundException : Exception
    {
        public string SettingName { get; }

        public SettingNotFoundException(string settingName)
            : base($"Setting not found : {settingName}")
        {
            SettingName = settingName;
        }
    }
}
