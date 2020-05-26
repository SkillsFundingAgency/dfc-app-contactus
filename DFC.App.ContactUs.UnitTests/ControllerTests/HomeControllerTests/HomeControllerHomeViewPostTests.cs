using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerHomeViewPostTests : BaseHomeControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedOptions => new List<object[]>
        {
            new object[] { HomeOption.Webchat, $"/{LocalPath}/{ChatController.ThisViewCanonicalName}" },
            new object[] { HomeOption.SendAMessage, $"/{LocalPath}/{WhyContactUsController.ThisViewCanonicalName}" },
            new object[] { HomeOption.Callback, $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}?{nameof(Category)}={Category.Callback}" },
            new object[] { HomeOption.Sendletter, $"/{LocalPath}/{HomeController.SendUsLetterCanonicalName}" },
        };

        public static IEnumerable<object[]> InvalidSelectedOptions => new List<object[]>
        {
            new object[] { HomeOption.None },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void HomeControllerHomeViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            string expectedRedirectUrl = $"/{LocalPath}/{ChatController.ThisViewCanonicalName}";
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

        [Theory]
        [MemberData(nameof(ValidSelectedOptions))]
        public void HomeControllerHomeViewPostRedirectsSuccessfully(HomeOption selectedOption, string expectedRedirectUrl)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = selectedOption,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.HomeView(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidSelectedOptions))]
        public void HomeControllerHomeViewPostReturnsSameViewForInvalidModel(HomeOption selectedOption)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = selectedOption,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

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
