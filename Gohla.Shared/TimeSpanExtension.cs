using System;

namespace Gohla.Shared
{
    /**
    TimeSpan extensions.
    **/
    public static class TimeSpanExtension
    {
        /**
        A TimeSpan extension method that converts a span to a readable string.
        
        @param  span    The span to act on.
        
        @return Readable string representation of the time span.
        **/
        public static String ToReadableString(this TimeSpan span)
        {
            if(span.Seconds <= 0)
                return "now";

            if(span.Days > 365)
                return "∞";

            String formatted = String.Format("{0}{1}{2}{3}",
                span.Days > 0 ? String.Format("{0:0}d, ", span.Days) : String.Empty,
                span.Hours > 0 ? String.Format("{0:0}h, ", span.Hours) : String.Empty,
                span.Minutes > 0 ? String.Format("{0:0}m, ", span.Minutes) : String.Empty,
                span.Seconds > 0 ? String.Format("{0:0}s", span.Seconds) : String.Empty);

            if(formatted.EndsWith(", ")) 
                formatted = formatted.Substring(0, formatted.Length - 2);

            return formatted;
        }

    }
}
