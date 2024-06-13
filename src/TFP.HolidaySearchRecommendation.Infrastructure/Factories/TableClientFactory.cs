using Azure.Data.Tables;
using Microsoft.Extensions.Options;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Factories
{
    public class TableClientFactory : ITableClientFactory
    {
        private readonly IOptions<DatabaseOptions> _databaseOptions;

        public TableClientFactory(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public async Task<TableClient> CreateTableClientAsync(string tableName)
        {
            var tableClient = new TableClient(_databaseOptions.Value.ConnectionString, tableName);

            return tableClient;
        }
    }
}
