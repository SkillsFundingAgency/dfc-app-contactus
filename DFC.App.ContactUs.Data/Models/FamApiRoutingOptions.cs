using DFC.App.ContactUs.Data.Models.ClientOptions;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class FamApiRoutingOptions : ClientOptionsModel
    {
        public string AreaRoutingEndpoint { get; set; } = "futureaccessmodel/arearouting/api/areas?location=";

        public string FallbackEmailToAddresses { get; set; } = string.Empty;

        public string ProblemsEmailAddress { get; set; } = string.Empty;

        public string FeebackEmailAddress { get; set; } = string.Empty;

        public string OtherEmailAddress { get; set; } = string.Empty;
    }
}
