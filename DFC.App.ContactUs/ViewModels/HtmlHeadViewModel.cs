using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.ContactUs.ViewModels
{
    public class HtmlHeadViewModel
    {
        public string? Title { get; set; }

        [Display(Name = "Canonical Name")]
        public Uri? CanonicalName { get; set; }

        public Uri? CanonicalUrl { get; set; }

        public string? Description { get; set; }

        public string? Keywords { get; set; }
    }
}
