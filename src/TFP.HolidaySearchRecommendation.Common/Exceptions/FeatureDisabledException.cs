namespace TFP.HolidaySearchRecommendation.Common.Exceptions
{
    public class FeatureDisabledException : Exception
    {
        public string FeatureName { get; }

        public FeatureDisabledException(string featureName)
            : base($"Feature not enabled : {featureName}")
        {
            FeatureName = featureName;
        }
    }
}
