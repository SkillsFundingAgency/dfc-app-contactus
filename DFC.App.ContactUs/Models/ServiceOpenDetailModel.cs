using System;
using System.Globalization;

namespace DFC.App.ContactUs.Models
{
    public class ServiceOpenDetailModel
    {
        public TimeSpan OpenTimeFrom { get; set; } = new TimeSpan(8, 0, 0);

        public TimeSpan OpenTimeTo { get; set; } = new TimeSpan(22, 0, 0);

        public string PhoneNumber { get; set; } = "0800 100 900";

        public DayOfWeek WeekdayFrom { get; set; } = DayOfWeek.Monday;

        public DayOfWeek WeekdayTo { get; set; } = DayOfWeek.Sunday;

        public string OpenDays
        {
            get
            {
                if (WeekdayFrom == DayOfWeek.Monday && WeekdayTo == DayOfWeek.Sunday)
                {
                    return "7 days a week";
                }

                return $"{WeekdayFrom} to {WeekdayTo}";
            }
        }

        public string OpenTimesString
        {
            get
            {
                return $"{OpenTimeFromString} to {OpenTimeToString}";
            }
        }

        public string OpenTimeFromString
        {
            get
            {
                return FormatTimeToString(OpenTimeFrom);
            }
        }

        public string OpenTimeToString
        {
            get
            {
                return FormatTimeToString(OpenTimeTo);
            }
        }

        public static string FormatTimeToString(TimeSpan timeSpan)
        {
            var timeAsDate = DateTime.Today.Add(timeSpan);

            string timeToString = timeSpan.Minutes != 0 ? timeAsDate.ToString("h:mm tt", CultureInfo.InvariantCulture) : timeAsDate.ToString("h tt", CultureInfo.InvariantCulture);

            timeToString = timeToString.Replace(" ", string.Empty, StringComparison.Ordinal).ToLowerInvariant();

            return timeToString;
        }
    }
}
