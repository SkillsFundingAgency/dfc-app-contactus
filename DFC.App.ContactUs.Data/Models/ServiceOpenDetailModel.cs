using System;
using System.Globalization;

namespace DFC.App.ContactUs.Data.Models
{
    public class ServiceOpenDetailModel
    {
        private string? linesOpenText;

        public TimeSpan OpenTimeFrom { get; set; } = new TimeSpan(8, 0, 0);

        public TimeSpan OpenTimeTo { get; set; } = new TimeSpan(22, 0, 0);

        public string PhoneNumber { get; set; } = "0800 100 900";

        public DayOfWeek WeekdayFrom { get; set; } = DayOfWeek.Monday;

        public DayOfWeek WeekdayTo { get; set; } = DayOfWeek.Sunday;

        public string LinesOpenText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(linesOpenText))
                {
                    return $"Lines are open from {OpenTimesString}, {OpenDays}.";
                }

                return linesOpenText!;
            }
            set => linesOpenText = value;
        }

        public string OpenDays
        {
            get
            {
                const int daysInWeek = 7;
                var previousWeekDay = (DayOfWeek)((((int)WeekdayFrom) - 1 + daysInWeek) % daysInWeek);

                if (previousWeekDay == WeekdayTo)
                {
                    return "7 days a week";
                }

                return $"{WeekdayFrom} to {WeekdayTo}";
            }
        }

        public string OpenTimesString => $"{OpenTimeFromString} to {OpenTimeToString}";

        public string OpenTimeFromString => FormatTimeToString(OpenTimeFrom);

        public string OpenTimeToString => FormatTimeToString(OpenTimeTo);

        public static string FormatTimeToString(TimeSpan timeSpan)
        {
            var timeAsDate = DateTime.Today.Add(timeSpan);

            string timeToString = timeSpan.Minutes != 0 ? timeAsDate.ToString("h:mm tt", CultureInfo.InvariantCulture) : timeAsDate.ToString("h tt", CultureInfo.InvariantCulture);

            timeToString = timeToString.Replace(" ", string.Empty, StringComparison.Ordinal).ToLowerInvariant();

            return timeToString;
        }

        public bool IsServiceOpenForDay(DayOfWeek dayOfWeek)
        {
            if (WeekdayFrom > WeekdayTo)
            {
                if (WeekdayFrom <= dayOfWeek || WeekdayTo >= dayOfWeek)
                {
                    return true;
                }

                return false;
            }

            if (WeekdayFrom <= dayOfWeek && WeekdayTo >= dayOfWeek)
            {
                return true;
            }

            return false;
        }
    }
}
