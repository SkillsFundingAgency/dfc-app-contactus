using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerWhyContactUsBodyTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerWhyContactUsBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBody();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerWhyContactUsBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBody();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerWhyContactUsBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsBody();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
