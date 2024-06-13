using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests
{
    public class GetHolidaySuggestionHttpRequest
    {
        /// <summary>
        /// Reference
        /// </summary>
        [Required]
        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
