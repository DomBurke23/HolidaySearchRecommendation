using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json.Linq;

namespace TFP.HolidaySearchRecommendation.Application.Services
{
    public interface IOrchestrationService
    {
        Task StartNewAsync(IDurableOrchestrationClient orchestrationClient, string orchestrator, string orchestrationId, JObject content);
    }
}
