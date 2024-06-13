using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TFP.HolidaySearchRecommendation.Common.Authorisation;
using TFP.HolidaySearchRecommendation.Common.Middleware;
using TFP.HolidaySearchRecommendation.Common.Options;
using TFP.HolidaySearchRecommendation.Common.Services;

namespace TFP.HolidaySearchRecommendation.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTenantServices(this IServiceCollection services)
        {
            services.AddScoped<ITenantContextAccessor, TenantContextAccessor>();
            services.AddScoped<ITenantMiddleware, TenantMiddleware>();
            services.AddOptions<List<TenantOptions>>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection("HolidaySearchRecommendation:Tenants")
                        .Bind(options);
                });
        }

        public static void AddFeatureServices(this IServiceCollection services)
        {
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddScoped<IFeatureMiddleware, FeatureMiddleware>();
        }

        public static void AddAuthorizationServices(this IServiceCollection services)
        {
            services.AddOptions<JwtBearerOptions>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.Authority = configuration["JwtBearer:Authority"];
                    options.Audience = configuration["JwtBearer:Audience"];
                    options.Issuer = configuration["JwtBearer:Issuer"];
                    options.ValidateLifetime = bool.Parse(configuration["JwtBearer:ValidateLifetime"]);
                });

            services.AddScoped<IJwtBearerValidator, JwtBearerValidator>();
            services.AddScoped<IAuthorizationMiddleware, AuthorizationMiddleware>();
            services.AddScoped<IUserContextAccessor, UserContextAccessor>();
        }

        public static void AddSettingService(this IServiceCollection services)
        {
            services.AddTransient<ISettingService, SettingService>();
        }
    }
}
