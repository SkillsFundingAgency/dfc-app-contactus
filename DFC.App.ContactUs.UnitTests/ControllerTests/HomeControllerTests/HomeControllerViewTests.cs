using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerViewTests : BaseHomeController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HomeControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void HomeControllerViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HomeControllerHomeViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeView();

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
