namespace TFP.HolidaySearchRecommendation.Common.Options
{
    public class JwtBearerOptions
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public bool ValidateLifetime { get; set; }
    }
}
