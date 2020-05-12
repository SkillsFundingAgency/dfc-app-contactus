using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerHomeViewTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerHomeViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerHomeViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void PagesControllerBHomeViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
