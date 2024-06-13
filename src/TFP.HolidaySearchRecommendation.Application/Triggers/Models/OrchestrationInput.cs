using Newtonsoft.Json.Linq;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.Models
{
    public class OrchestrationInput
    {
        public string Tenant { get; set; }
        public JObject Data { get; set; }
    }
}
