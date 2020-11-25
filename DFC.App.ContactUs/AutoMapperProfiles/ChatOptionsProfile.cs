using AutoMapper;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class ChatOptionsProfile : Profile
    {
        public ChatOptionsProfile()
        {
            CreateMap<ChatOptions, ChatViewBodyModel>()
                .ForMember(d => d.PhoneNumber, s => s.Ignore())
                .ForMember(d => d.HowCanWeHelpLink, s => s.Ignore());
        }
    }
}
