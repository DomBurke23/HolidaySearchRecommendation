using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TFP.HolidaySearchRecommendation.Application;
using TFP.HolidaySearchRecommendation.Application.Extensions;
using TFP.HolidaySearchRecommendation.Common.Extensions;
using TFP.HolidaySearchRecommendation.Infrastructure.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TFP.HolidaySearchRecommendation.Application
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // Microsoft recommends Environment.GetEnvironmentVariable as both work locally with local.settings.json and in Azure.
            /*builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("AzureAppConfiguration:ConnectionString"))
                .Select("HolidaySearchRecommendation:*")
               // Configure to reload configuration if the registered sentinel key is modified
               .ConfigureRefresh(refreshOptions =>
                  refreshOptions.Register("HolidaySearchRecommendation:Sentinel", refreshAll: true));
            });*/

        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddApplicationServices();
            builder.Services.AddEventServices();
            builder.Services.AddMessageServices();
            builder.Services.AddDataServices();
            builder.Services.AddTenantServices();
            builder.Services.AddAuthorizationServices();
            builder.Services.AddFeatureServices();
            builder.Services.AddSettingService();
        }
    }
}
