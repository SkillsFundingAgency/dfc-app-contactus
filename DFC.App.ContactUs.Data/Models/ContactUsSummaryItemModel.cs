﻿using DFC.App.ContactUs.Data.Contracts;
using System;

namespace DFC.App.ContactUs.Data.Models
{
    public class ContactUsSummaryItemModel : IApiDataModel
    {
        public Uri? Url { get; set; }

        public string? CanonicalName { get; set; }

        public DateTime Published { get; set; }
    }
}
