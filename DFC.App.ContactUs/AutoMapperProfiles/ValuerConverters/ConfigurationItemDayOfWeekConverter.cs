using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using System;

namespace DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters
{
    public class ConfigurationItemDayOfWeekConverter : IValueConverter<ConfigurationItemApiDataModel?, DayOfWeek>
    {
        public DayOfWeek Convert(ConfigurationItemApiDataModel? sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrWhiteSpace(sourceMember?.Value))
            {
                return default;
            }

            if (Enum.TryParse(sourceMember.Value, out DayOfWeek weekDayFrom))
            {
                return weekDayFrom;
            }

            return default;
        }
    }
}
