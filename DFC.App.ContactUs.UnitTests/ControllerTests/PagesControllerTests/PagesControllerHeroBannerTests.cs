using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerHeroBannerTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerHeroBannerJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HeroBanner(article);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerHeroBannerWithNullArticleJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const string article = null;
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HeroBanner(article);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
