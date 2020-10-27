using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using System;

namespace DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters
{
    public class ConfigurationItemTimeSpanConverter : IValueConverter<ConfigurationItemApiDataModel?, TimeSpan>
    {
        public TimeSpan Convert(ConfigurationItemApiDataModel? sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember?.Value))
            {
                return default;
            }

            if (TimeSpan.TryParse(sourceMember.Value, out TimeSpan openTimeTo))
            {
                return openTimeTo;
            }

            return default;
        }
    }
}
