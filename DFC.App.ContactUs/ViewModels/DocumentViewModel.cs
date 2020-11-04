<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
=======
﻿using System;
>>>>>>> story/DFCC-1169-refresh-nugets
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class DocumentViewModel
    {
        public HtmlHeadViewModel? HtmlHead { get; set; }

        public BreadcrumbViewModel? Breadcrumb { get; set; }

        [Display(Name = "Document Id")]
<<<<<<< HEAD
        public Guid? DocumentId { get; set; }

        [Display(Name = "Canonical Name")]
        public string? CanonicalName { get; set; }
=======
        public Guid Id { get; set; }

        public string? Title { get; set; }
>>>>>>> story/DFCC-1169-refresh-nugets

        [Display(Name = "PartitionKey")]
        public string? PartitionKey { get; set; }

<<<<<<< HEAD
        [Required]
        public Guid? Version { get; set; }

        [Display(Name = "Breadcrumb Title")]
        public string? BreadcrumbTitle { get; set; }

        [Display(Name = "Include In SiteMap")]
        public bool IncludeInSitemap { get; set; }

        public Uri? Url { get; set; }

        public HtmlString? Content { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [Display(Name = "Alternative Names")]
        public IList<string>? AlternativeNames { get; set; }
=======
        public Uri? Url { get; set; }

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Last Cached")]
        public DateTime LastCached { get; set; }
>>>>>>> story/DFCC-1169-refresh-nugets

        public BodyViewModel? BodyViewModel { get; set; }
    }
}
