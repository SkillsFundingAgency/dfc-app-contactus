using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class RoutingDetailModel
    {
        public string? TouchpointID { get; set; }

        public string? Area { get; set; }

        public string? TelephoneNumber { get; set; }

        public string? SMSNumber { get; set; }

        public string? EmailAddress { get; set; }
    }
}
