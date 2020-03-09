// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Utils
{
    public static class TimeUtils
    {
        private static readonly DateTime UnixDate = new DateTime(1970, 1, 1, 0, 0, 0);
        
        /// <summary>
        /// Return unix time in seconds
        /// </summary>
        public static Int64 UnixTime()
        {
            TimeSpan timeSpan = DateTime.Now - UnixDate;
            return (Int64)timeSpan.TotalSeconds;
        }
        
        /// <summary>
        /// Return unix time in seconds
        /// </summary>
        /// <param name="milli">Return unix time in milliseconds</param>
        public static Int64 UnixTime(Boolean milli)
        {
            if (!milli)
            {
                return UnixTime();
            }

            TimeSpan timeSpan = DateTime.Now - UnixDate;
            return (Int64)timeSpan.TotalMilliseconds;
        }
        
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
    }
}