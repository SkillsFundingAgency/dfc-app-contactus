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
    public class SessionStateGetTests
    {
        private readonly ILogger<EnterYourDetailsController> logger;

        private readonly ISessionStateService<SessionDataModel> fakeSessionStateService;

        private readonly IMapper fakeMapper;

        private readonly INotifyEmailService<ContactUsEmailRequestModel> fakeNotifyEmailService;

        private readonly IRoutingService fakeRoutingService;

        private readonly FamApiRoutingOptions fakeFamApiRoutingOptions;

        private readonly ITemplateService fakeTemplateService;

        public SessionStateGetTests()
        {
            logger = A.Fake<ILogger<EnterYourDetailsController>>();
            fakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            fakeMapper = A.Fake<AutoMapper.IMapper>();
            fakeRoutingService = A.Fake<IRoutingService>();
            fakeNotifyEmailService = A.Fake<INotifyEmailService<ContactUsEmailRequestModel>>();
            fakeFamApiRoutingOptions = A.Fake<FamApiRoutingOptions>();
            fakeTemplateService = A.Fake<ITemplateService>();
        }

        [Fact]
        public async Task SessionStateGetWithValidSessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, Guid.NewGuid().ToString());

            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateGetWithEmptySessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            controller.Request.Headers.Add(ConstantStrings.CompositeSessionIdHeaderName, string.Empty);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Fact]
        public async Task SessionStateGetWithMissingSessionIdHeaderReturnsSuccessForCallback()
        {
            // Arrange
            var controller = BuildEnterYourDetailsController(MediaTypeNames.Text.Html);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => fakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        private EnterYourDetailsController BuildEnterYourDetailsController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new EnterYourDetailsController(logger, fakeMapper, fakeSessionStateService, fakeRoutingService, fakeNotifyEmailService, fakeFamApiRoutingOptions)
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
