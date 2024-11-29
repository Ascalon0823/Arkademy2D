using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Arkademy.Common
{
    [Serializable]
    public class GameTime
    {
        public int weeks;
        public int dayOfWeek;
        public int hour;

        [JsonIgnore] public Action<GameTime> OnGameTimeChanged;
        [JsonIgnore] public Action<int> OnNewDay;
        [JsonIgnore] public Action<int> OnNewWeek;

        public void UpdateTime()
        {
            var newDay = false;
            var newWeek = false;
            if (hour >= 16)
            {
                dayOfWeek += hour / 16;
                hour = hour % 16;
                newDay = true;
            }

            if (dayOfWeek >= 7)
            {
                weeks += dayOfWeek / 7;
                dayOfWeek %= 7;
                newWeek = true;
            }

            OnGameTimeChanged?.Invoke(this);
            if (newDay) OnNewDay?.Invoke(dayOfWeek);
            if (newWeek) OnNewWeek?.Invoke(weeks);
        }

        public void AddHour(int hours)
        {
            hour += hours;
            UpdateTime();
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
            return $"W{weeks + 1}";
        }
    }
}