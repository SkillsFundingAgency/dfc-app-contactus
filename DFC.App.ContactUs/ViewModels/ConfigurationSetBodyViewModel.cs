using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class ConfigurationSetBodyViewModel
    {
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Lines Open String")]
        public string? LinesOpenString { get; set; }

        [Display(Name = "Lines Open Built String")]
        public string? LinesOpenBuiltString { get; set; }

        [Display(Name = "Lines Open Text")]
        public string? LinesOpenText { get; set; }

        [Display(Name = "Open Time From")]
        public TimeSpan OpenTimeFrom { get; set; }

        [Display(Name = "Open Time To")]
        public TimeSpan OpenTimeTo { get; set; }

        [Display(Name = "Weekday From")]
        public DayOfWeek WeekdayFrom { get; set; }

        [Display(Name = "Weekday To")]
        public DayOfWeek WeekdayTo { get; set; }

        [Display(Name = "Open Days")]
        public string? OpenDays { get; set; }

        [Display(Name = "Open Times String")]
        public string? OpenTimesString { get; set; }

        [Display(Name = "Open Time From String")]
        public string? OpenTimeFromString { get; set; }

        [Display(Name = "Open Time To String")]
        public string? OpenTimeToString { get; set; }
    }
}
