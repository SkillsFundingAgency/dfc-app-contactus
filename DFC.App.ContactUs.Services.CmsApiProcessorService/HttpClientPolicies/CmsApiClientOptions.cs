using DFC.App.ContactUs.Data.Models;

namespace DFC.App.ContactUs.Services.CmsApiProcessorService.HttpClientPolicies
{
    public class CmsApiClientOptions : ClientOptionsModel
    {
        public string SummaryEndpoint { get; set; } = "api/index";
    }
}
