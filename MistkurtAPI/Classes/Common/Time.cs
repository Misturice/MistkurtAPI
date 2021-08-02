using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MistkurtAPI.Classes.Common
{
    public static class Time
    {
        public static long GetTodayTimestamp()
        {
            DateTimeOffset today = (DateTimeOffset)DateTime.Today.ToUniversalTime();
            return today.ToUnixTimeSeconds();
        }

        public static long GetEndOfDayTimestamp()
        {
            DateTimeOffset endOfDay = (DateTimeOffset)DateTime.Today.ToUniversalTime().AddSeconds(24*60*60-1);
            return endOfDay.ToUnixTimeSeconds();
        }

        public static long GetCurrentTimestamp()
        {
            DateTimeOffset now = (DateTimeOffset)DateTime.Now.ToUniversalTime();
            return now.ToUnixTimeSeconds();
        }

    }
}
