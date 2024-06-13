namespace TFP.HolidaySearchRecommendation.Domain.Messaging.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Tenant { get; set; }
        public object Data { get; set; }
    }
}
