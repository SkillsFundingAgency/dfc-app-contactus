using DFC.Compui.Cosmos.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationSetModel : DocumentModel
    {
        public const string DefaultPartitionKey = "configurationSet";

        public override string? PartitionKey { get; set; } = DefaultPartitionKey;

        public string? Title { get; set; }

        [Required]
        public Uri? Url { get; set; }

        public string? PhoneNumber { get; set; }

        public string? LinesOpenText { get; set; }

        public TimeSpan? OpenTimeFrom { get; set; }

        public TimeSpan? OpenTimeTo { get; set; }

        public DayOfWeek? WeekdayFrom { get; set; }

        public DayOfWeek? WeekdayTo { get; set; }

        public DateTime LastReviewed { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastCached { get; set; } = DateTime.UtcNow;
    }
}
