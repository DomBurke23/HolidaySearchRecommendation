namespace TFP.HolidaySearchRecommendation.Domain.Exceptions
{
    public class ModelSaveException : Exception
    {
        public string ClientRequestId { get; }
        public string Reason { get; }

        public ModelSaveException(string clientRequestId, string reason)
            : base($"Error executing table client, client request Id: {clientRequestId}, reason :{reason}")
        {
            ClientRequestId = clientRequestId;
            Reason = reason;
        }
    }
}
