using System;

namespace TFP.HolidaySearchRecommendation.Application.UseCases.Operations.Requests
{
    public class FinishOperationRequest
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Requester { get; set; }
        public int ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}
