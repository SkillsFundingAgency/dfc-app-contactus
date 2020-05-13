using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Option", "Home Controller Unit Tests")]
    public class HomeControllerBodyPostTests : BaseHomeController
    {
        public static IEnumerable<object[]> ValidSelectedOptions => new List<object[]>
        {
            new object[] { HomeBodyViewModel.SelectOption.Webchat, $"/{HomeController.WebchatRegistrationPath}/chat", },
            new object[] { HomeBodyViewModel.SelectOption.SendAMessage, $"/{HomeController.RegistrationPath}/why-do-you-want-to-contact-us", },
            new object[] { HomeBodyViewModel.SelectOption.Callback, $"/{HomeController.RegistrationPath}/contact-us", },
            new object[] { HomeBodyViewModel.SelectOption.Sendletter, $"/{HomeController.RegistrationPath}/send-us-a-letter", },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public void HomeControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            string expectedRedirectUrl = $"/{HomeController.WebchatRegistrationPath}/chat";
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeBodyViewModel.SelectOption.Webchat,
            };
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeBody(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(ValidSelectedOptions))]
        public void HomeControllerBodyPostReturnsSuccessForValidOptions(HomeBodyViewModel.SelectOption selectedOption, string expectedRedirectUrl)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = selectedOption,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            // Act
            var result = controller.HomeBody(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public void HomeControllerBodyPostReturnsErrorForInvalidOption()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeBodyViewModel.SelectOption.None,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(HomeBodyViewModel.SelectedOption), "Fake error");

            // Act
            var result = controller.HomeBody(viewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);
            Assert.True(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.Contains(nameof(HomeBodyViewModel.SelectedOption), viewResult.ViewData.ModelState.Keys);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public void HomeControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeBodyViewModel.SelectOption.None,
            };
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.HomeBody(viewModel);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
