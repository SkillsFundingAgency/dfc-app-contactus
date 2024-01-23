using System.Diagnostics.CodeAnalysis;

using DFC.Content.Pkg.Netcore.Data.Models;

namespace DFC.App.ContactUs.Data.Models.CmsApiModels
{
    [ExcludeFromCodeCoverage]
    public class SharedContentItemApiDataModel : BaseContentItemModel
    {
        public string? Content { get; set; }
    }
}
