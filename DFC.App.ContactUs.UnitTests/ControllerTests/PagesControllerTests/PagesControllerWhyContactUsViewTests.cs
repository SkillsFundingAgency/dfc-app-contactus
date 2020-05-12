using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerWhyContactUsViewTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerWhyContactUsViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerWhyContactUsViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsView();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<WhyContactUsViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerBWhyContactUsViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.WhyContactUsView();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
