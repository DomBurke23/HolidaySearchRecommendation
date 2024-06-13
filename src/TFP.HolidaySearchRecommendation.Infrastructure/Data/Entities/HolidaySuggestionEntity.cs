using Azure;
using Azure.Data.Tables;


namespace TFP.HolidaySearchRecommendation.Infrastructure.Data.Entities
{
    public class HolidaySuggestionEntity : ITableEntity
    {
        public HolidaySuggestionEntity()
        {

        }

        public HolidaySuggestionEntity(string tenant, string reference)
        {
            PartitionKey = tenant;
            RowKey = reference;
        }


        public string Name { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
