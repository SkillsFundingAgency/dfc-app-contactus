using AutoMapper;
using DFC.App.ContactUs.Data.Models;

namespace DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters
{
    public class ConfigurationItemStringConverter : IValueConverter<ConfigurationItemApiDataModel?, string?>
    {
        public string? Convert(ConfigurationItemApiDataModel? sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember?.Value))
            {
                return null;
            }

            return sourceMember.Value;
        }
    }
}
