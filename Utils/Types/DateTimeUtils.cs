// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using System.ComponentModel;
using System.Globalization;

namespace Common_Library.Utils
{
    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    
    public static class DateTimeUtils
    {
        public static DateTime DateTime(this Month month, Int32 year, Int32 day, Int32 hour = 0, Int32 minute = 0, Int32 second = 0, Int32 millisecond = 0, DateTimeKind kind = DateTimeKind.Unspecified)
        {
            return new DateTime(year, (Int32) month, day, hour, minute, second, millisecond, kind);
        }
        
        public static DateTime DateTime(this Month month, Int32 year, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond, Calendar calendar)
        {
            return new DateTime(year, (Int32) month, day, hour, minute, second, millisecond, calendar);
        }

        public static DateTime DateTime(this Month month, Int32 year, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond, Calendar calendar, DateTimeKind kind)
        {
            return new DateTime(year, (Int32) month, day, hour, minute, second, millisecond, calendar, kind);
        }
    }
}