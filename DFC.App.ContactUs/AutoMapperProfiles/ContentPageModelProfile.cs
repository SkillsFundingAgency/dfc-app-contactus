using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using DFC.App.ContactUs.ViewModels;
using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ContentPageModelProfile : Profile
    {
        public ContentPageModelProfile()
        {
            CreateMap<ContentPageModel, BodyViewModel>()
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)));

            CreateMap<ChatOptions, ChatViewBodyModel>();

            CreateMap<ContentPageModel, DocumentViewModel>()
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.Id))
                .ForMember(d => d.HtmlHead, s => s.Ignore())
                .ForMember(d => d.Breadcrumb, s => s.Ignore())
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)))
                .ForMember(d => d.BodyViewModel, s => s.MapFrom(a => a));

            CreateMap<ContentPageModel, HtmlHeadViewModel>()
                .ForMember(d => d.CanonicalUrl, s => s.Ignore())
                .ForMember(d => d.Title, s => s.MapFrom(a => a.MetaTags != null ? a.MetaTags.Title + " | " + PagesController.ContactUsPageTitleSuffix : PagesController.ContactUsPageTitleSuffix))
                .ForMember(d => d.Description, s => s.MapFrom(a => a.MetaTags != null ? a.MetaTags.Description : null))
                .ForMember(d => d.Keywords, s => s.MapFrom(a => a.MetaTags != null ? a.MetaTags.Keywords : null));

            CreateMap<ContentPageModel, IndexDocumentViewModel>();

            CreateMap<ContentPageModel, BreadcrumbItemModel>();

            CreateMap<ContactUsApiDataModel, ContentPageModel>()
                .ForMember(d => d.Id, s => s.MapFrom(a => a.ItemId))
                .ForMember(d => d.Etag, s => s.Ignore())
                .ForMember(d => d.PartitionKey, s => s.Ignore())
                .ForMember(d => d.Content, opt => opt.ConvertUsing(new ContentItemsConverter(), a => a.ContentItems))
                .ForPath(d => d.LastReviewed, s => s.MapFrom(a => a.Published))
                .ForPath(d => d.MetaTags.Title, s => s.MapFrom(a => a.Title))
                .ForPath(d => d.MetaTags.Description, s => s.MapFrom(a => a.Description))
                .ForPath(d => d.MetaTags.Keywords, s => s.MapFrom(a => a.Keywords));
        }
    }
}
