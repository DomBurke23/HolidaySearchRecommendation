namespace TFP.HolidaySearchRecommendation.Domain.Messaging.Content
{
    public class StartOperationMessageContent
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Requester { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
