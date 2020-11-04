using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationItemApiDataModel : BaseContentItemModel
    {
        public string? Value { get; set; }
    }
}
