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

                return $"{WeekdayFrom.ToString()} to {WeekdayTo.ToString()}";
            }
        }

        public string OpenTimes
        {
            get
            {
                var openTimeFromAsDate = DateTime.Today.Add(OpenTimeFrom);
                var openTimeToAsDate = DateTime.Today.Add(OpenTimeTo);

                string openTimeFromString = OpenTimeFrom.Minutes != 0 ? openTimeFromAsDate.ToString("h:mm tt", CultureInfo.InvariantCulture) : openTimeFromAsDate.ToString("h tt", CultureInfo.InvariantCulture);
                string openTimeToString = OpenTimeTo.Minutes != 0 ? openTimeToAsDate.ToString("h:mm tt", CultureInfo.InvariantCulture) : openTimeToAsDate.ToString("h tt", CultureInfo.InvariantCulture);

                openTimeFromString = openTimeFromString.Replace(" ", string.Empty).ToLowerInvariant();
                openTimeToString = openTimeToString.Replace(" ", string.Empty).ToLowerInvariant();

                return $"{openTimeFromString} to {openTimeToString}";
            }
        }
    }
}
