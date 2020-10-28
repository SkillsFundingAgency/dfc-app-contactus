using DFC.App.ContactUs.Data.Contracts;
using DFC.Compui.Cosmos.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationSetModel : DocumentModel, ICachedModel
    {
        public const string DefaultPartitionKey = "configurationSet";

        public override string? PartitionKey { get; set; } = DefaultPartitionKey;

        [Required]
        public string? Title { get; set; }

        [Required]
        public Uri? Url { get; set; }

        public string? PhoneNumber { get; set; }

        public string? LinesOpenText { get; set; }

        public TimeSpan OpenTimeFrom { get; set; }

        public TimeSpan OpenTimeTo { get; set; }

        public DayOfWeek WeekdayFrom { get; set; }

        public DayOfWeek WeekdayTo { get; set; }

        public DateTime LastReviewed { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastCached { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public string LinesOpenString => string.IsNullOrWhiteSpace(LinesOpenText) ? $"Lines are open from {OpenTimesString}, {OpenDays}." : LinesOpenText;

        [JsonIgnore]
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

        [JsonIgnore]
        public string OpenTimesString => $"{OpenTimeFromString} to {OpenTimeToString}";

        [JsonIgnore]
        public string OpenTimeFromString => FormatTimeToString(OpenTimeFrom);

        [JsonIgnore]
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
