﻿using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerViewPostTests : BaseEnterYourDetailsControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task EnterYourDetailsControllerViewPostReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            const bool expectedSendEmailResult = true;
            string expectedRedirectUrl = $"/{PagesController.LocalPath}/{HomeController.ThankyouForContactingUsCanonicalName}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).Returns(A.Dummy<string>());
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).Returns(A.Fake<ContactUsEmailRequestModel>());
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).Returns(expectedSendEmailResult);

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public async Task EnterYourDetailsControllerViewPostReturnsSameViewForInvalidModel()
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task EnterYourDetailsControllerViewPostReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var viewModel = new EnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            controller.ModelState.AddModelError(nameof(EnterYourDetailsBodyViewModel.SelectedCategory), "Fake error");

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustNotHaveHappened();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }

        [Fact]
        public async Task EnterYourDetailsControllerViewPostReturnsSuccessForMissingRouting()
        {
            // Arrange
            const bool expectedSendEmailResult = true;
            RoutingDetailModel? expectedRoutingDetailModel = null;
            string expectedRedirectUrl = $"/{PagesController.LocalPath}/{HomeController.ThankyouForContactingUsCanonicalName}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).Returns(A.Dummy<string>());
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).Returns(A.Fake<ContactUsEmailRequestModel>());
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).Returns(expectedSendEmailResult);

            // Act
            var result = await controller.EnterYourDetailsView(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeRoutingService.GetEmailToSendTo(A<string>.Ignored, A<Category>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeNotifyEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }
    }
}
