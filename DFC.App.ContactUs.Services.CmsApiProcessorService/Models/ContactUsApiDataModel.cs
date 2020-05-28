﻿using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using System;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService.Models
{
    public class ContactUsApiDataModel : IApiDataModel
    {
        public Guid? ItemId { get; set; }

        public string? CanonicalName { get; set; }

        public Guid? Version { get; set; }

        public string? BreadcrumbTitle { get; set; }

        public bool IncludeInSitemap { get; set; }

        public Uri? Url { get; set; }

        public IList<string>? AlternativeNames { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Keywords { get; set; }

        public string? Content { get; set; }

        public DateTime? Published { get; set; }
    }
}
