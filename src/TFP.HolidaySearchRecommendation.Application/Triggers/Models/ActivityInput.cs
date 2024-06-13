namespace TFP.HolidaySearchRecommendation.Application.Triggers.Models
{
    public class ActivityInput<T>
    {
        public string Tenant { get; set; }
        public T Request { get; set; }
    }
}
