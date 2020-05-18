using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerHomeViewPostTests : BaseHomeController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void HomeControllerHomeViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            string expectedRedirectUrl = $"/{PagesController.LocalPath}";
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Webchat,
            };
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeView(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public void HomeControllerHomeViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel();
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(HomeBodyViewModel.SelectedOption), "Fake error");

            // Act
            var result = controller.HomeView(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HomeControllerHomeViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.None,
            };
            var controller = BuildHomeController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(HomeBodyViewModel.SelectedOption), "Fake error");

            // Act
            var result = controller.HomeView(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
