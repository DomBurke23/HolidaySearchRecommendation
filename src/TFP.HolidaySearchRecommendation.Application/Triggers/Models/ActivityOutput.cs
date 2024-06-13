namespace TFP.HolidaySearchRecommendation.Application.Triggers.Models
{
    public class ActivityOutput<T>
    {
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public T Response { get; set; }
    }
}
