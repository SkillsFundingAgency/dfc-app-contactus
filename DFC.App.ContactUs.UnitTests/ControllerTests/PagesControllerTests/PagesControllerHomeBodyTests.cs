using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerHomeBodyTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerHomeBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeBody();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerHomeBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeBody();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerHomeBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeBody();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
