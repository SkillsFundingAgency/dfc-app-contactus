using DFC.App.ContactUs.Data.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Data.Models
{
    public class ContentPageModel : IDataModel
    {
        [Guid]
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string? Etag { get; set; }

        [Required]
        [LowerCase]
        [UrlPath]
        public string? CanonicalName { get; set; }

        public string PartitionKey => "static-page";

        public long SequenceNumber { get; set; }

        [Display(Name = "Breadcrumb Title")]
        public string? BreadcrumbTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        [UrlPath]
        [LowerCase]
        public IList<string>? AlternativeNames { get; set; }

        public MetaTagsModel MetaTags { get; set; } = new MetaTagsModel();

        public string? Content { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }
    }
}
