using DFC.App.ContactUs.Services.ApiProcessorService.Contracts;
using System;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService.Models
{
    public class ContactUsSummaryItemModel : IApiDataModel
    {
        public Uri? Url { get; set; }

        public string? CanonicalName { get; set; }

        public DateTime Published { get; set; }
    }
}
