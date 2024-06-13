using Azure.Data.Tables;
using TFP.HolidaySearchRecommendation.Infrastructure.Constants;
using TFP.HolidaySearchRecommendation.Infrastructure.Factories;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Data.Migrations
{
    public class SchemaMigration001 : ISchemaMigration
    {
        private readonly ITableClientFactory _tableClientFactory;

        public SchemaMigration001(ITableClientFactory tableClientFactory)
        {
            _tableClientFactory = tableClientFactory;
        }

        public async Task ApplyAsync()
        {
            TableClient cloudTableClient = await _tableClientFactory.CreateTableClientAsync(TableConstants.HolidaySuggestion);
            await cloudTableClient.CreateIfNotExistsAsync();
        }
    }
}
