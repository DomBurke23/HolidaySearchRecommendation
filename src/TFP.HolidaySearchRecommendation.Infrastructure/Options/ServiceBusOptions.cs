namespace TFP.HolidaySearchRecommendation.Infrastructure.Options
{
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }
        public string ApplicationTopic { get; set; }
        public string OperationsTopic { get; set; }
    }
}
