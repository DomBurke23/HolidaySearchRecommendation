using Microsoft.Azure.WebJobs;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.WarmUp
{
    public class WarmUpTrigger
    {
        [FunctionName("WarmUpTrigger")]
        public void Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {
            // Used to keep underlying function app instance warm
            // https://mikhail.io/serverless/coldstarts/azure/
            // https://markheath.net/post/avoiding-azure-functions-cold-starts
        }
    }
}
