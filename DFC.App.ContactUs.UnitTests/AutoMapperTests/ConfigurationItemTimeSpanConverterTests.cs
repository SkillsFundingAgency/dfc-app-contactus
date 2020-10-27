using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Data.Models;
using System;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.AutoMapperTests
{
    [Trait("Category", "AutoMapper")]
    public class ConfigurationItemTimeSpanConverterTests
    {
        [Fact]
        public void ConfigurationItemTimeSpanConverterReturnsSuccessForValidSourceMemberValue()
        {
            // Arrange
            var expectedResult = new TimeSpan(10, 30, 0);
            var converter = new ConfigurationItemTimeSpanConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = expectedResult.ToString() };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemTimeSpanConverterReturnsNullForNullSourceMember()
        {
            // Arrange
            var expectedResult = new TimeSpan(0);
            var converter = new ConfigurationItemTimeSpanConverter();
            ConfigurationItemApiDataModel? sourceMember = null;
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemTimeSpanConverterReturnsNullForNullSourceMemberValue()
        {
            // Arrange
            var expectedResult = new TimeSpan(0);
            var converter = new ConfigurationItemTimeSpanConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = null };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemTimeSpanConverterReturnsNullForInvalidSourceMemberValue()
        {
            // Arrange
            var expectedResult = new TimeSpan(0);
            var converter = new ConfigurationItemTimeSpanConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = "not a week day" };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
