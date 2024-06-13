namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    public interface IFeatureMiddleware
    {
        void Invoke(string featureName);
    }
}
