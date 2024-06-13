using TFP.HolidaySearchRecommendation.Common.Exceptions;
using TFP.HolidaySearchRecommendation.Common.Services;

namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    internal class FeatureMiddleware : IFeatureMiddleware
    {
        private readonly IFeatureService _featureService;

        public FeatureMiddleware(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        public void Invoke(string featureName)
        {
            if (!_featureService.IsEnabled(featureName))
            {
                throw new FeatureDisabledException(featureName);
            }
        }
    }
}
