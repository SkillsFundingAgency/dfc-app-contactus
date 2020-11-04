using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DocumentViewModel
    {
        public HtmlHeadViewModel? HtmlHead { get; set; }

        public BreadcrumbViewModel? Breadcrumb { get; set; }

        [Display(Name = "Document Id")]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        [Display(Name = "Partition Key")]
        public string? PartitionKey { get; set; }

        public Uri? Url { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Cached")]
        public DateTime LastCached { get; set; }

        public ConfigurationSetBodyViewModel? ConfigurationSetBodyViewModel { get; set; }

        public EmailBodyViewModel? EmailBodyViewModel { get; set; }
    }
}
