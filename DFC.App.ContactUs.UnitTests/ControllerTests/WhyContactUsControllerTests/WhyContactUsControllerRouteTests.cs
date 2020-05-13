﻿using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WhyContactUsControllerTests
{
    [Trait("Category", "WhyContactUs Controller Unit Tests")]
    public class WhyContactUsControllerRouteTests
    {
        private const string WhyContactUsArticleName = "why-do-you-want-to-contact-us";

        private readonly ILogger<WhyContactUsController> logger;

        public WhyContactUsControllerRouteTests()
        {
            logger = A.Fake<ILogger<WhyContactUsController>>();
        }

        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/pages/{WhyContactUsArticleName}",  nameof(WhyContactUsController.WhyContactUsView) },
            new object[] { $"/pages/{WhyContactUsArticleName}/htmlhead",  nameof(WhyContactUsController.WhyContactUsHtmlHead) },
            new object[] { $"/pages/{WhyContactUsArticleName}/breadcrumb", nameof(WhyContactUsController.WhyContactUsBreadcrumb) },
            new object[] { $"/pages/{WhyContactUsArticleName}/body", nameof(WhyContactUsController.WhyContactUsBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public void WhyContactUsControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private IActionResult RunControllerAction(WhyContactUsController controller, string actionName)
        {
            return actionName switch
            {
                nameof(WhyContactUsController.WhyContactUsHtmlHead) => controller.WhyContactUsHtmlHead(),
                nameof(WhyContactUsController.WhyContactUsBreadcrumb) => controller.WhyContactUsBreadcrumb(),
                _ => controller.WhyContactUsBody(),
            };
        }

        private WhyContactUsController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new WhyContactUsController(logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}