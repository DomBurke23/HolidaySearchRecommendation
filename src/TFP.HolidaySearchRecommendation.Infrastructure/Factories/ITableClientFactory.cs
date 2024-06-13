using Azure.Data.Tables;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Factories
{
    public interface ITableClientFactory
    {
        Task<TableClient> CreateTableClientAsync(string tableName);
    }
}
