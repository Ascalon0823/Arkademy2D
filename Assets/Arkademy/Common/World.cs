using System;
using System.Globalization;

namespace Arkademy.Common
{
    [Serializable]
    public class GameTime
    {
        public int weeks;
        public int dayOfWeek;
        public int hour;

        public void UpdateTime()
        {
            if (hour >= 16)
            {
                dayOfWeek += hour / 16;
                hour = hour % 16;
            }

            if (dayOfWeek >= 7)
            {
                weeks += dayOfWeek / 7;
                dayOfWeek %= 7;
            }
        }

        public string HourString()
        {
            return $"{hour + 6:00}:00";
        }
        
        public string DayString()
        {
            return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName((DayOfWeek)dayOfWeek);
        }

        public string WeekString()
        {
            return $"W{weeks}";
        }
    }
}