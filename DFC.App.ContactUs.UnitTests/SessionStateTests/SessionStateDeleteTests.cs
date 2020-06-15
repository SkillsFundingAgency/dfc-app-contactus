using AutoMapper;
using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.SessionStateTests
{
    [Trait("Category", "Session State Unit Tests")]
    public class SessionStateDeleteTests
    {
        private const string RegistrationPath = "contact-us";

        private readonly ILogger<EnterYourDetailsController> logger;

        private readonly ISessionStateService<SessionDataModel> fakeSessionStateService;

        private readonly IMapper fakeMapper;

        private readonly ISendGridEmailService<ContactUsEmailRequestModel> fakeSendGridEmailService;

        private readonly IRoutingService fakeRoutingService;

        private readonly FamApiRoutingOptions fakeFamApiRoutingOptions;

        private readonly ITemplateService fakeTemplateService;

        public SessionStateDeleteTests()
        {
            logger = A.Fake<ILogger<EnterYourDetailsController>>();
            fakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            fakeMapper = A.Fake<AutoMapper.IMapper>();
            fakeRoutingService = A.Fake<IRoutingService>();
            fakeSendGridEmailService = A.Fake<ISendGridEmailService<ContactUsEmailRequestModel>>();
            fakeFamApiRoutingOptions = A.Fake<FamApiRoutingOptions>();
            fakeTemplateService = A.Fake<ITemplateService>();
        }

        [Fact]
        public async Task SessionStateDeleteWithValidSessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            const string expectedEmailTemplate = "An email template";
            const bool expectedSendEmailResult = true;
            string expectedRedirectUrl = $"/{RegistrationPath}/{HomeController.ThankyouForContactingUsCanonicalName}";
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, Guid.NewGuid().ToString());

            A.CallTo(() => fakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).Returns(expectedEmailTemplate);
            A.CallTo(() => fakeRoutingService.GetAsync(A<string>.Ignored)).Returns(A.Dummy<RoutingDetailModel>());
            A.CallTo(() => fakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).Returns(A.Fake<ContactUsEmailRequestModel>());
            A.CallTo(() => fakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).Returns(expectedSendEmailResult);
            A.CallTo(() => fakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await controller.EnterYourDetailsBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeTemplateService.GetTemplateByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeRoutingService.GetAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContactUsEmailRequestModel>(A<EnterYourDetailsBodyViewModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSendGridEmailService.SendEmailAsync(A<ContactUsEmailRequestModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal(expectedRedirectUrl, redirectResult.Url);
            Assert.False(redirectResult.Permanent);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateDeleteWithEmptySessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, string.Empty);

            // Act
            var result = await controller.EnterYourDetailsBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateDeleteWithMissingSessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var viewModel = ValidModelBuilders.BuildValidEnterYourDetailsBodyViewModel();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            // Act
            var result = await controller.EnterYourDetailsBody(viewModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        private EnterYourDetailsController BuildEnterYourDetailsController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new EnterYourDetailsController(logger, fakeMapper, fakeSessionStateService, fakeRoutingService, fakeSendGridEmailService, fakeFamApiRoutingOptions, fakeTemplateService)
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
