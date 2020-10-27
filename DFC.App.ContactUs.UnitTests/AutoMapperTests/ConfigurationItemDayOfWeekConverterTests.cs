using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Data.Models;
using System;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.AutoMapperTests
{
    [Trait("Category", "AutoMapper")]
    public class ConfigurationItemDayOfWeekConverterTests
    {
        [Fact]
        public void ConfigurationItemDayOfWeekConverterReturnsSuccessForValidSourceMemberValue()
        {
            // Arrange
            const DayOfWeek expectedResult = DayOfWeek.Wednesday;
            var converter = new ConfigurationItemDayOfWeekConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = expectedResult.ToString() };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemDayOfWeekConverterReturnsNullForNullSourceMember()
        {
            // Arrange
            const DayOfWeek expectedResult = DayOfWeek.Sunday;
            var converter = new ConfigurationItemDayOfWeekConverter();
            ConfigurationItemApiDataModel? sourceMember = null;
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemDayOfWeekConverterReturnsNullForNullSourceMemberValue()
        {
            // Arrange
            const DayOfWeek expectedResult = DayOfWeek.Sunday;
            var converter = new ConfigurationItemDayOfWeekConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = null };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemDayOfWeekConverterReturnsNullForInvalidSourceMemberValue()
        {
            // Arrange
            const DayOfWeek expectedResult = DayOfWeek.Sunday;
            var converter = new ConfigurationItemDayOfWeekConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = "not a week day" };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
