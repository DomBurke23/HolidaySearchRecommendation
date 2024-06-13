using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json.Linq;
using TFP.HolidaySearchRecommendation.Application.Triggers.Models;
using TFP.HolidaySearchRecommendation.Common.Services;

namespace TFP.HolidaySearchRecommendation.Application.Services
{
    public class OrchestrationService : IOrchestrationService
    {
        private readonly ITenantContextAccessor _tenantContextAccessor;

        public OrchestrationService(ITenantContextAccessor tenantContextAccessor)
        {
            _tenantContextAccessor = tenantContextAccessor;
        }

        public async Task StartNewAsync(IDurableOrchestrationClient orchestrationClient, string orchestrator, string orchestrationId, JObject data)
        {
            var orchestrationInput = new OrchestrationInput()
            {
                Tenant = _tenantContextAccessor.TenantContext.TenantOptions.Name,
                Data = data
            };

            await orchestrationClient.StartNewAsync(orchestrator, orchestrationId, orchestrationInput);
        }
    }
}
