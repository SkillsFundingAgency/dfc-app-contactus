using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerBodyPostTests : BaseHomeControllerTests
    {
        public static IEnumerable<object[]> ValidSelectedOptions => new List<object[]>
        {
            new object[] { HomeOption.Webchat, $"/{HomeController.WebchatRegistrationPath}/{ChatController.ThisViewCanonicalName}" },
            new object[] { HomeOption.SendAMessage, $"/{HomeController.RegistrationPath}/{HowCanWeHelpController.ThisViewCanonicalName}" },
            new object[] { HomeOption.Callback, $"/{HomeController.RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}" },
            new object[] { HomeOption.Sendletter, $"/{HomeController.RegistrationPath}/{HomeController.SendUsLetterCanonicalName}" },
        };

        public static IEnumerable<object[]> InvalidSelectedOptions => new List<object[]>
        {
            new object[] { HomeOption.None },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HomeControllerBodyPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            string expectedRedirectUrl = $"/{HomeController.RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}";
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Callback,
            };
            var controller = BuildHomeController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(ValidSelectedOptions))]
        public async Task HomeControllerBodyPostReturnsSuccessForValidOptions(HomeOption selectedOption, string expectedRedirectUrl)
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = selectedOption,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            if (selectedOption == HomeOption.Callback)
            {
                A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
                A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(HttpStatusCode.OK);
            }

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            if (selectedOption == HomeOption.Callback)
            {
                A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
                A.CallTo(() => FakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustHaveHappenedOnceExactly();
            }

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidSelectedOptions))]
        public async Task HomeControllerBodyPostReturnsSameViewForInvalidModel(HomeOption selectedOption)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = selectedOption,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HomeControllerBodyPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.None,
            };
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
