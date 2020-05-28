using DFC.App.ContactUs.Data.Models;

namespace DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies
{
    public class FamApiRoutingOptions : ClientOptionsModel
    {
        public string AreaRoutingEndpoint { get; set; } = "futureaccessmodel/arearouting/api/areas?location=";

        public string FallbackEmailToAddresses { get; set; } = "digital.first.careers@gmail.com";

        public string NoReplyEmailAddress { get; set; } = "no-reply@nationalcareers.service.gov.uk";
    }
}
