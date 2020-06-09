using DFC.App.ContactUs.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Data.Models
{
    public class ContentPageModel : DFC.Compui.Cosmos.Models.ContentPageModel, IEventDataModel
    {
        [Required]
        [JsonProperty(Order = -10)]
        public override string PartitionKey => "static-page";

        [Obsolete("May be removed once Service Bus and Message Function app removed from solution")]
        public long SequenceNumber { get; set; }

        [Required]
        public Guid? Version { get; set; }

        [Required]
        [Display(Name = "Breadcrumb Title")]
        public string? BreadcrumbTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        [Required]
        public Uri? Url { get; set; }

        public MetaTagsModel MetaTags { get; set; } = new MetaTagsModel();

        [Required]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }
    }
}
