using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using TFP.HolidaySearchRecommendation.Application.Constants;
using TFP.HolidaySearchRecommendation.Infrastructure.Factories;
using TFP.HolidaySearchRecommendation.Infrastructure.Services;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.SchemaMigration
{
    public class SchemaMigrationTrigger
    {
        private readonly ITableClientFactory _tableClientFactory;
        private readonly ISchemaMigrationService _schemaMigrationService;

        public SchemaMigrationTrigger(ITableClientFactory tableClientFactory,
            ISchemaMigrationService schemaMigrationService)
        {
            _tableClientFactory = tableClientFactory;
            _schemaMigrationService = schemaMigrationService;
        }

        /// <summary>
        /// RunOnStartup is set to true, so this function will run during startup to create the databse and run migrations
        /// </summary>
        [FunctionName("SchemaMigrationTrigger")]
        public async Task Run([TimerTrigger(CrontabConstants.ImpossibleSchedule, RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            try
            {
                // TODO : Move retry config to appsettings
                await Policy
                    .Handle<SqlException>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAsync(async () =>
                    {
                        log.LogInformation($"Ensuring schema migrations are up to date");

                        await _schemaMigrationService.ApplyAsync();
                    });
            }
            catch (Exception)
            {
                log.LogError($"Failed to ensure schema migrations were up to date");
                throw;
            }
        }
    }
}
