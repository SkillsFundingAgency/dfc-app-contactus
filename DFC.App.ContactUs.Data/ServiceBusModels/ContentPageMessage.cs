using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.Data.ServiceBusModels
{
    public class ContentPageMessage : BaseContentPageMessage
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public DateTime? LastModified { get; set; }

        [Required]
        public string Title { get; set; }

        public string BreadcrumbTitle { get; set; }

        public string[] AlternativeNames { get; set; }

        public bool IncludeInSitemap { get; set; }

        public string Description { get; set; }

        public string Keywords { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
