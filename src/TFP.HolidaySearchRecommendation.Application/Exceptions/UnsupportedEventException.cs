using System;

namespace TFP.HolidaySearchRecommendation.Application.Exceptions
{
    public class UnsupportedEventException : Exception
    {
        public string Name { get; }

        public UnsupportedEventException(string eventName)
            : base($"Trigger does not support event {eventName}")
        {
            Name = eventName;
        }
    }
}
