using DFC.App.ContactUs.Data.Models.ClientOptions;
<<<<<<< HEAD

namespace DFC.App.ContactUs.Data.Models
{
=======
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class FamApiRoutingOptions : ClientOptionsModel
    {
        public string AreaRoutingEndpoint { get; set; } = "futureaccessmodel/arearouting/api/areas?location=";

        public string FallbackEmailToAddresses { get; set; } = "digital.first.careers@gmail.com";

        public string NoReplyEmailAddress { get; set; } = "no-reply@nationalcareers.service.gov.uk";
    }
}
