// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;

namespace Common_Library.Time
{
    public static class Time
    {
        public const Int32 MilliInSecond = 1000;
        
        public const Int32 SecondsInMinute = 60;
        
        public const Int32 MinutesInHour = 60;
        
        public const Int32 HoursInDay = 24;
        
        public const Int32 DaysInWeek = 7;
        
        public const Int32 WeeksInMonth = 4;
        public const Int32 DaysInMonth = 30;

        public const Int32 MonthsInYear = 12;
        public const Int32 WeeksInYear = 52;
        public const Int32 DaysInYear = 365;
        
        public const Int32 YearsInCentury = 100;
        public const Int32 YearsInMillennium = 1000;

        public static class Year
        {
            public static readonly TimeSpan One = TimeSpan.FromDays(DaysInYear);
            public static readonly TimeSpan Two = TimeSpan.FromDays(DaysInYear * 2);
            public static readonly TimeSpan Three = TimeSpan.FromDays(DaysInYear * 3);
            public static readonly TimeSpan Five = TimeSpan.FromDays(DaysInYear * 5);
            public static readonly TimeSpan Ten = TimeSpan.FromDays(DaysInYear * 10);
            public static readonly TimeSpan Century = TimeSpan.FromDays(DaysInYear * YearsInCentury);
            public static readonly TimeSpan Millennium = TimeSpan.FromDays(DaysInYear * YearsInMillennium);
        }

        public static class Month
        {
            public static readonly TimeSpan One = TimeSpan.FromDays(DaysInMonth);
            public static readonly TimeSpan Three = TimeSpan.FromDays(DaysInMonth * 3);
            public static readonly TimeSpan Six = TimeSpan.FromDays(DaysInMonth * 6);
            public static readonly TimeSpan Nine = TimeSpan.FromDays(DaysInMonth * 9);
            public static readonly TimeSpan Twelve = TimeSpan.FromDays(DaysInMonth * 12);
        }
        
        public static class Week
        {
            public static readonly TimeSpan One = TimeSpan.FromDays(DaysInWeek);
            public static readonly TimeSpan Two = TimeSpan.FromDays(DaysInWeek * 2);
            public static readonly TimeSpan Three = TimeSpan.FromDays(DaysInWeek * 3);
            public static readonly TimeSpan Fourth = TimeSpan.FromDays(DaysInWeek * 4);

            public static TimeSpan Get(Int32 count)
            {
                return TimeSpan.FromDays(DaysInWeek * count);
            }
        }

        public static class Day
        {
            public static readonly TimeSpan One = TimeSpan.FromDays(1);
            public static readonly TimeSpan ThreeQuarter = TimeSpan.FromDays(0.75);
            public static readonly TimeSpan Half = TimeSpan.FromDays(0.5);
            public static readonly TimeSpan Quarter = TimeSpan.FromDays(0.25);
            
            public static TimeSpan Get(Int32 count)
            {
                return TimeSpan.FromDays(count);
            }
        }

        public static class Hour
        {
            public static readonly TimeSpan One = TimeSpan.FromHours(1);
            public static readonly TimeSpan ThreeQuarter = TimeSpan.FromHours(0.75);
            public static readonly TimeSpan Half = TimeSpan.FromHours(0.5);
            public static readonly TimeSpan Quarter = TimeSpan.FromHours(0.25);
            
            public static TimeSpan Get(Int32 count)
            {
                return TimeSpan.FromHours(count);
            }
        }

        public static class Minute
        {
            public static readonly TimeSpan One = TimeSpan.FromMinutes(1);
            public static readonly TimeSpan ThreeQuarter = TimeSpan.FromMinutes(0.75);
            public static readonly TimeSpan Half = TimeSpan.FromMinutes(0.5);
            public static readonly TimeSpan Quarter = TimeSpan.FromMinutes(0.25);
            
            public static TimeSpan Get(Int32 count)
            {
                return TimeSpan.FromMinutes(count);
            }
        }

        public static class Second
        {
            public static readonly TimeSpan One = TimeSpan.FromSeconds(1);
            public static readonly TimeSpan ThreeQuarter = TimeSpan.FromSeconds(0.75);
            public static readonly TimeSpan Half = TimeSpan.FromSeconds(0.5);
            public static readonly TimeSpan Quarter = TimeSpan.FromSeconds(0.25);
            
            public static TimeSpan Get(Int32 count)
            {
                return TimeSpan.FromMilliseconds(count);
            }
        }
    }
}