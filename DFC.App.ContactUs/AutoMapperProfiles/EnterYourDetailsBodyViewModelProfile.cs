using AutoMapper;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class EnterYourDetailsBodyViewModelProfile : Profile
    {
        public EnterYourDetailsBodyViewModelProfile()
        {
            CreateMap<EnterYourDetailsBodyViewModel, ContactUsEmailRequestModel>()
                .ForMember(d => d.GivenName, s => s.MapFrom(a => a.FirstName))
                .ForMember(d => d.FamilyName, s => s.MapFrom(a => a.LastName))
                .ForMember(d => d.DateOfBirth, s => s.MapFrom(a => a.DateOfBirth != null ? a.DateOfBirth.Value : default))
                .ForMember(d => d.CallbackDateTime, s => s.MapFrom(a => a.CallbackDateOptionSelected.HasValue && a.CallbackTimeOptionSelected.HasValue ? $"{EnterYourDetailsBodyViewModel.DateLabels[a.CallbackDateOptionSelected.Value]}, {a.CallbackTimeOptionSelected.Value.GetDescription()}" : string.Empty))
                .ForMember(d => d.Query, s => s.MapFrom(a => a.MoreDetail))
                .ForMember(d => d.Subject, s => s.MapFrom(a => a.SelectedCategory.GetDescription()))
                .ForMember(d => d.FromEmailAddress, s => s.MapFrom(a => a.EmailAddress))
                .ForMember(d => d.ToEmailAddress, s => s.Ignore())
                .ForMember(d => d.Body, s => s.Ignore())
                .ForMember(d => d.BodyNoHtml, s => s.Ignore())
                .ForMember(d => d.TokenValueMappings, s => s.Ignore());
        }
    }
}
