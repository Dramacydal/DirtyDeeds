using System;

namespace DD.Extensions
{
    public static class DateTimeExtensions
    {
        public static long MSecToNow(this DateTime date)
        {
            return (DateTime.Now.Ticks - date.Ticks) / TimeSpan.TicksPerMillisecond;
        }

        public static bool Passed(this DateTime date)
        {
            return date <= DateTime.Now;
        }
    }
}
