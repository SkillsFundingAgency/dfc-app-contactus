using DFC.App.ContactUs.Data.Models.ClientOptions;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class FamApiRoutingOptions : ClientOptionsModel
    {
        public string AreaRoutingEndpoint { get; set; } = "futureaccessmodel/arearouting/api/areas?location=";

        public string FallbackEmailToAddresses { get; set; } = "digital.first.careers@gmail.com";

        public string ProblemsEmailAddress { get; set; } = "no-reply@nationalcareers.service.gov.uk";

        public string FeebackEmailAddress { get; set; } = "no-reply@nationalcareers.service.gov.uk";

        public string OtherEmailAddress { get; set; } = "no-reply@nationalcareers.service.gov.uk";
    }
}
