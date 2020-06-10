using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.AreaRoutingService.Contracts;
using DFC.App.ContactUs.Services.AreaRoutingService.HttpClientPolicies;
using DFC.App.ContactUs.Services.EmailService.Contracts;
using DFC.App.ContactUs.Services.EmailTemplateService.Contracts;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    public abstract class BaseEnterYourDetailsControllerTests
    {
        protected const string LocalPath = "pages";
        protected const string RegistrationPath = "contact-us";

        protected BaseEnterYourDetailsControllerTests()
        {
            Logger = A.Fake<ILogger<EnterYourDetailsController>>();
            FakeSessionStateService = A.Fake<ISessionStateService<SessionDataModel>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeRoutingService = A.Fake<IRoutingService>();
            FakeSendGridEmailService = A.Fake<ISendGridEmailService<ContactUsEmailRequestModel>>();
            FakeFamApiRoutingOptions = A.Fake<FamApiRoutingOptions>();
            FakeTemplateService = A.Fake<ITemplateService>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*/*" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected ILogger<EnterYourDetailsController> Logger;

        protected ISessionStateService<SessionDataModel> FakeSessionStateService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected ISendGridEmailService<ContactUsEmailRequestModel> FakeSendGridEmailService { get; }

        protected IRoutingService FakeRoutingService { get; }

        protected FamApiRoutingOptions FakeFamApiRoutingOptions { get; }

        protected ITemplateService FakeTemplateService { get; }

        protected EnterYourDetailsController BuildEnterYourDetailsController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new EnterYourDetailsController(Logger, FakeMapper, FakeSessionStateService, FakeRoutingService, FakeSendGridEmailService, FakeFamApiRoutingOptions, FakeTemplateService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            controller.Request.Headers.Add(Constants.CompositeSessionIdHeaderName, Guid.NewGuid().ToString());

            return controller;
        }
    }
}
