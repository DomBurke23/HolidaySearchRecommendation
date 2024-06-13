using System;

namespace TFP.HolidaySearchRecommendation.Application.Exceptions
{
    public class BadMessageException : Exception
    {
        public BadMessageException() { }

        public BadMessageException(string message)
            : base(message) { }

        public BadMessageException(string message, Exception inner)
            : base(message, inner) { }
    }
}
