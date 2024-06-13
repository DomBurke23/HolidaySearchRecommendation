using Azure.Data.Tables;
using TFP.HolidaySearchRecommendation.Common.Services;
using TFP.HolidaySearchRecommendation.Domain.Data.Repositories;
using TFP.HolidaySearchRecommendation.Domain.Exceptions;
using TFP.HolidaySearchRecommendation.Domain.Models;
using TFP.HolidaySearchRecommendation.Infrastructure.Constants;
using TFP.HolidaySearchRecommendation.Infrastructure.Data.Entities;
using TFP.HolidaySearchRecommendation.Infrastructure.Factories;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Data.Repositories
{
    public class HolidaySuggestionRepository : IHolidaySuggestionRepository
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;
        private readonly ITableClientFactory _tableClientFactory;

        public HolidaySuggestionRepository(ITableClientFactory tableClientFactory,
            ITenantContextAccessor tenantContextAccessor)
        {
            _tableClientFactory = tableClientFactory;
            _tenantContextAccessor = tenantContextAccessor;
        }

        public async Task SaveAsync(HolidaySuggestion holidaySuggestion)
        {
            var holidaySuggestionEntity = new HolidaySuggestionEntity(_tenantContextAccessor.TenantContext.TenantOptions.Name, holidaySuggestion.Reference)
            {
                Name = holidaySuggestion.Name
            };

            TableClient tableClient = await _tableClientFactory.CreateTableClientAsync(TableConstants.HolidaySuggestion);
            var result = await tableClient.UpsertEntityAsync(holidaySuggestionEntity);

            if (result.IsError)
            {
                throw new ModelSaveException(result.ClientRequestId, result.ReasonPhrase);
            }
        }

        public async Task<HolidaySuggestion> FindAsync(string reference)
        {
            var partitionKey = _tenantContextAccessor.TenantContext.TenantOptions.Name;
            var rowId = reference;
            TableClient tableClient = await _tableClientFactory.CreateTableClientAsync(TableConstants.HolidaySuggestion);

            var entity = await tableClient.GetEntityIfExistsAsync<HolidaySuggestionEntity>(partitionKey, rowId);
            if (entity is null || !entity.HasValue)
            {
                return null;
            }


            var holidaySuggestion = new HolidaySuggestion()
            {
                Reference = entity.Value.RowKey,
                Name = entity.Value.Name
            };

            return holidaySuggestion;
        }
    }
}
