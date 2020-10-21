using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ServiceOpenDetailModelProfile : Profile
    {
        public ServiceOpenDetailModelProfile()
        {
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
        }
    }
}
