using TFP.HolidaySearchRecommendation.Infrastructure.Data.Migrations;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Services
{
    public class SchemaMigrationService : ISchemaMigrationService
    {
        private readonly IEnumerable<ISchemaMigration> _schemaMigrations;

        public SchemaMigrationService(IEnumerable<ISchemaMigration> schemaMigrations)
        {
            _schemaMigrations = schemaMigrations;
        }

        public async Task ApplyAsync()
        {
            // Services should appear in the order they were registered when resolved via IEnumerable<{SERVICE}> but ensuring correct order of execution
            var ascendingSchemaMigrations = _schemaMigrations.OrderBy(sm => sm.GetType().Name).ToList();

            foreach (ISchemaMigration schemaMigration in ascendingSchemaMigrations)
            {
                await schemaMigration.ApplyAsync();
            }
        }
    }
}
