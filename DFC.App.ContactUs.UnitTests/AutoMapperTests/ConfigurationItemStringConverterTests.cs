using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Data.Models;
using System;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.AutoMapperTests
{
    [Trait("Category", "AutoMapper")]
    public class ConfigurationItemStringConverterTests
    {
        [Fact]
        public void ConfigurationItemStringConverterReturnsSuccessForValidSourceMemberValue()
        {
            // Arrange
            const string expectedResult = "some data or other";
            var converter = new ConfigurationItemStringConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = expectedResult };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemStringConverterReturnsNullForNullSourceMember()
        {
            // Arrange
            const string? expectedResult = null;
            var converter = new ConfigurationItemStringConverter();
            ConfigurationItemApiDataModel? sourceMember = null;
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConfigurationItemStringConverterReturnsNullForNullSourceMemberValue()
        {
            // Arrange
            const string? expectedResult = null;
            var converter = new ConfigurationItemStringConverter();
            var sourceMember = new ConfigurationItemApiDataModel { Value = null };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}
