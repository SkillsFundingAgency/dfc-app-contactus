using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ConfigurationSetModelProfile : Profile
    {
        public ConfigurationSetModelProfile()
        {
            CreateMap<ConfigurationSetApiDataModel, ConfigurationSetModel>()
                .ForMember(d => d.Id, s => s.MapFrom(a => a.ItemId))
                .ForMember(d => d.Etag, s => s.Ignore())
                .ForMember(d => d.ParentId, s => s.Ignore())
                .ForMember(d => d.TraceId, s => s.Ignore())
                .ForMember(d => d.PartitionKey, s => s.Ignore())
                .ForMember(d => d.PhoneNumber, opt => opt.ConvertUsing(new ConfigurationItemStringConverter(), a => a.ContentItems.OfType<ConfigurationItemApiDataModel>().FirstOrDefault(f => string.Compare(f.Title, "Telephone number", true, CultureInfo.InvariantCulture) == 0)))
                .ForMember(d => d.LinesOpenText, opt => opt.ConvertUsing(new ConfigurationItemStringConverter(), a => a.ContentItems.OfType<ConfigurationItemApiDataModel>().FirstOrDefault(f => string.Compare(f.Title, "Lines open text", true, CultureInfo.InvariantCulture) == 0)))
                .ForMember(d => d.LastReviewed, s => s.MapFrom(a => a.Published))
                .ForMember(d => d.LastCached, s => s.Ignore());

            CreateMap<LinkDetails, ConfigurationItemApiDataModel>()
                .ForMember(d => d.Url, s => s.Ignore())
                .ForMember(d => d.ItemId, s => s.Ignore())
                .ForMember(d => d.Title, s => s.Ignore())
                .ForMember(d => d.Published, s => s.Ignore())
                .ForMember(d => d.CreatedDate, s => s.Ignore())
                .ForMember(d => d.Value, s => s.Ignore())
                .ForMember(d => d.Links, s => s.Ignore())
                .ForMember(d => d.ContentLinks, s => s.Ignore())
                .ForMember(d => d.ContentItems, s => s.Ignore());

            CreateMap<ConfigurationSetModel, IndexDocumentViewModel>();

            CreateMap<ConfigurationSetModel, DocumentViewModel>()
                .ForMember(d => d.HtmlHead, s => s.MapFrom(a => a))
                .ForMember(d => d.Breadcrumb, s => s.Ignore())
                .ForMember(d => d.EmailBodyViewModel, s => s.Ignore());

            CreateMap<ConfigurationSetModel, HtmlHeadViewModel>()
                .ForMember(d => d.CanonicalUrl, s => s.Ignore())
                .ForMember(d => d.Description, s => s.Ignore())
                .ForMember(d => d.Keywords, s => s.Ignore());

            CreateMap<ConfigurationSetModel, BreadcrumbItemModel>()
                .ForMember(d => d.Route, s => s.Ignore());
        }
    }
}
