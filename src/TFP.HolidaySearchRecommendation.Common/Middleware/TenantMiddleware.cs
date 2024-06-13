using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Common.Constants;
using TFP.HolidaySearchRecommendation.Common.Context;
using TFP.HolidaySearchRecommendation.Common.Exceptions;
using TFP.HolidaySearchRecommendation.Common.Options;
using TFP.HolidaySearchRecommendation.Common.Services;

namespace TFP.HolidaySearchRecommendation.Common.Middleware
{
    public class TenantMiddleware : ITenantMiddleware
    {
        private readonly IOptionsSnapshot<List<TenantOptions>> _tenantsOptions;
        private readonly ITenantContextAccessor _tenantContextAccessor;

        public TenantMiddleware(IOptionsSnapshot<List<TenantOptions>> tenantsOptions,
            ITenantContextAccessor tenantContextAccessor)
        {
            _tenantsOptions = tenantsOptions;
            _tenantContextAccessor = tenantContextAccessor;
        }

        public async Task InvokeAsync(string tenant)
        {
            SetTenant(tenant);
        }

        public async Task InvokeAsync(IDictionary<string, object> applicationProperties)
        {
            applicationProperties.TryGetValue(CustomHeaderConstants.Tenant, out var _tenant);
            var tenant = (string)_tenant;

            SetTenant(tenant);
        }

        public async Task InvokeAsync(EventGridEvent eventGridEvent)
        {
            var content = JObject.Parse(eventGridEvent.Data.ToString());
            var tenant = (string)content["Tenant"];

            SetTenant(tenant);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue(HttpRequestHeadersConstants.XTenant, out StringValues xTenant);
            var tenant = (string)xTenant;

            SetTenant(tenant);
        }

        private void SetTenant(string tenant)
        {
            if (string.IsNullOrWhiteSpace(tenant))
            {
                throw new TenantNotFoundException($"Tenant not provided");
            }

            TenantOptions tenantOptions = _tenantsOptions.Value.FirstOrDefault(to => to.Name == tenant);

            if (tenantOptions == null)
            {
                throw new TenantNotFoundException($"Couldn't find tenant {tenant}");
            }

            _tenantContextAccessor.TenantContext = new TenantContext() { TenantOptions = tenantOptions };
        }
    }
}
