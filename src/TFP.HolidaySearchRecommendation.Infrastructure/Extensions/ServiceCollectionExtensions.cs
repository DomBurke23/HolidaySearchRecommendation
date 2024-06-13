using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TFP.HolidaySearchRecommendation.Domain.Data.Repositories;
using TFP.HolidaySearchRecommendation.Domain.Services;
using TFP.HolidaySearchRecommendation.Infrastructure.Data.Migrations;
using TFP.HolidaySearchRecommendation.Infrastructure.Data.Repositories;
using TFP.HolidaySearchRecommendation.Infrastructure.Factories;
using TFP.HolidaySearchRecommendation.Infrastructure.Options;
using TFP.HolidaySearchRecommendation.Infrastructure.Services;

namespace TFP.HolidaySearchRecommendation.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddOptions<DatabaseOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.ConnectionString = configuration["Database:ConnectionString"];
                });

            services.AddTransient<ISchemaMigration, SchemaMigration001>();
            services.AddTransient<ISchemaMigrationService, SchemaMigrationService>();
            services.AddTransient<IHolidaySuggestionRepository, HolidaySuggestionRepository>();
            services.AddTransient<ITableClientFactory, TableClientFactory>();
        }

        public static void AddMessageServices(this IServiceCollection services)
        {
            services.AddOptions<ServiceBusOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.ConnectionString = configuration["AzureWebJobsServiceBus"];
                    options.ApplicationTopic = configuration["AzureServiceBus:Topics:Application"];
                    options.OperationsTopic = configuration["AzureServiceBus:Topics:Operations"];
                });

            services.AddScoped<IMessageService, ServiceBusService>();
        }

        public static void AddEventServices(this IServiceCollection services)
        {
            services.AddOptions<EventGridOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.Endpoint = configuration["AzureEventGrid:Endpoint"];
                    options.AccessKey = configuration["AzureEventGrid:AccessKey"];
                });

            services.AddScoped<IEventService, EventGridService>();
        }
    }
}
