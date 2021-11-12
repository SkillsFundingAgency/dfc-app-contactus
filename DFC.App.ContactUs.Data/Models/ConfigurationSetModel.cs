using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationSetModel
    {
        public string? PhoneNumber { get; private set; } = "0800 100 900";

        public string? LinesOpenText { get; set; }
    }
}
