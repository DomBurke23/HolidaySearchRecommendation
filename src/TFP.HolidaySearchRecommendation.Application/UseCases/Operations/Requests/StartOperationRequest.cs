using System;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Requests
{
    public class StartOperationRequest
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Requester { get; set; }
        public DateTime StartedAt { get; set; }
    }
}
