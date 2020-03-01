// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class TimeUtils
    {
        private static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0);
        
        public static Int64 UnixTime(this DateTime time)
        {
            if (time <= DateTime.MinValue)
            {
                return 0;
            }

            if (time < UnixDate)
            {
                return 0;
            }

            TimeSpan timeSpan = time - UnixDate;
            return (Int64)timeSpan.TotalSeconds;
        }

        public static DateTime UnixTime(Int64 time)
        {
            return time == 0 ? DateTime.MinValue : UnixDate.AddSeconds(time);
        }

        public static Int64 UnixTimeNow()
        {
            TimeSpan timeSpan = DateTime.Now - UnixDate;
            return (Int64)timeSpan.TotalSeconds;
        }

        public static Int64 UnixTimeNowInMilli()
        {
            TimeSpan timeSpan = DateTime.Now - UnixDate;
            return (Int64)timeSpan.TotalMilliseconds;
        }
    }
}