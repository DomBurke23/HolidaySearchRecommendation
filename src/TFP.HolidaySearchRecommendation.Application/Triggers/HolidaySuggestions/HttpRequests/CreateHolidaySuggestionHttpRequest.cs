using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace TFP.HolidaySearchRecommendation.Application.Triggers.HolidaySuggestions.HttpRequests
{
    public class CreateHolidaySuggestionHttpRequest
    {
        /// <summary>
        /// Reference
        /// </summary>
        [Required]
        [JsonProperty("reference")]
        public string Reference { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
