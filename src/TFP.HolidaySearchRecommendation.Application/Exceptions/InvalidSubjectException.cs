namespace TFP.HolidaySearchRecommendation.Application.Exceptions
{
    public class InvalidSubjectException : BadMessageException
    {
        public string Subject { get; }

        public InvalidSubjectException(string subject)
            : base($"Invalid subject {subject}")
        {
            Subject = subject;
        }
    }
}
