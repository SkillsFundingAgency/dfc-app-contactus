using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.PageService;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HealthControllerTests
{
    public class BaseHealthController
    {
        public BaseHealthController()
        {
            FakeContentPageService = A.Fake<IContentPageService>();
            FakeLogger = A.Fake<ILogger<HealthController>>();
        }

        protected IContentPageService FakeContentPageService { get; }

        protected ILogger<HealthController> FakeLogger { get; }

        protected HealthController BuildHealthController()
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            var controller = new HealthController(FakeLogger, FakeContentPageService)
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
