﻿using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Enums;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.SessionStateTests
{
    [Trait("Category", "Session State Unit Tests")]
    public class SessionStateSetTests
    {
        private readonly ILogger<HomeController> logger;

        private readonly ISessionStateService<SessionDataModel> fakeSessionStateService;

        private readonly ServiceOpenDetailModel fakeServiceOpenDetailModel;

        public SessionStateSetTests()
        {
            logger = A.Fake<ILogger<HomeController>>();
            fakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            fakeServiceOpenDetailModel = A.Fake<ServiceOpenDetailModel>();
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        public async Task SessionStateSetWithValidSessionIdHeaderReturnsSuccessForCallback(HttpStatusCode saveStatusCode)
        {
            // Arrange
            var expectedRedirectUrl = $"/{HomeController.RegistrationPath}/{EnterYourDetailsController.ThisViewCanonicalName}";
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Callback,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, Guid.NewGuid().ToString());

            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);
            A.CallTo(() => fakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).Returns(saveStatusCode);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateSetWithEmptySessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Callback,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, string.Empty);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateSetWithMissingSessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var viewModel = new HomeBodyViewModel
            {
                SelectedOption = HomeOption.Callback,
            };
            var controller = BuildHomeController(MediaTypeNames.Text.Html);

            // Act
            var result = await controller.HomeBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeSessionStateService.SaveAsync(A<SessionStateModel<SessionDataModel>>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        private HomeController BuildHomeController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new HomeController(logger, fakeSessionStateService, fakeServiceOpenDetailModel)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
