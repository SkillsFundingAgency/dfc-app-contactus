using AutoMapper;
using DFC.App.ContactUs.AutoMapperProfiles.ValuerConverters;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using System.Collections.Generic;
using Xunit;

namespace DFC.App.ContactUs.UnitTests
{
    [Trait("Category", "Automapper")]
    public class ContentItemsConverterTests
    {
        [Fact]
        public void ContentItemsConverterTestsReturnsSuccess()
        {
            // Arrange
            const string expectedResult = "<div class=\"govuk-grid-column-one-half\">this is content</div>";
            var converter = new ContentItemsConverter();
            IList<ContactUsApiContentItemModel> sourceMember = new List<ContactUsApiContentItemModel>
            {
                new ContactUsApiContentItemModel
                {
                    Ordinal = 1,
                    Justify = 1,
                    Width = 50,
                    Content = "this is content",
                },
            };
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ContentItemsConverterTestsReturnsNullForNullSourceMember()
        {
            // Arrange
            var converter = new ContentItemsConverter();
            IList<ContactUsApiContentItemModel>? sourceMember = null;
            var context = new ResolutionContext(null, null);

            // Act
            var result = converter.Convert(sourceMember, context);

            // Assert
            Assert.Null(result);
        }
    }
}
