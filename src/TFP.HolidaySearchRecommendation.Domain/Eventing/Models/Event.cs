namespace TFP.HolidaySearchRecommendation.Domain.Eventing.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
    }
}
