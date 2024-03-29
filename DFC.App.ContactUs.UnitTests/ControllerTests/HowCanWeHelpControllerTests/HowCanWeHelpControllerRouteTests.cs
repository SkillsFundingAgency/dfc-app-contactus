﻿using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HowCanWeHelpControllerTests
{
    [Trait("Category", "HowCanWeHelp Controller Unit Tests")]
    public class HowCanWeHelpControllerRouteTests : BaseHowCanWeHelpControllerTests
    {
        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}",  nameof(HowCanWeHelpController.HowCanWeHelpView) },
            new object[] { $"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}/head",  nameof(HowCanWeHelpController.HowCanWeHelpHead) },
            new object[] { $"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}/breadcrumb", nameof(HowCanWeHelpController.HowCanWeHelpBreadcrumb) },
            new object[] { $"/{LocalPath}/{HowCanWeHelpController.ThisViewCanonicalName}/body", nameof(HowCanWeHelpController.HowCanWeHelpBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public async Task HowCanWeHelpControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private async Task<IActionResult> RunControllerAction(HowCanWeHelpController controller, string actionName)
        {
            return actionName switch
            {
                nameof(HowCanWeHelpController.HowCanWeHelpHead) => controller.HowCanWeHelpHead(),
                nameof(HowCanWeHelpController.HowCanWeHelpBreadcrumb) => controller.HowCanWeHelpBreadcrumb(),
                _ => await controller.HowCanWeHelpBody().ConfigureAwait(false),
            };
        }

        private HowCanWeHelpController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new HowCanWeHelpController(Logger, FakeSessionStateService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}