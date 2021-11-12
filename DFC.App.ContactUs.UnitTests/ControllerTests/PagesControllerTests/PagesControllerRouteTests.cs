using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerRouteTests : BasePagesControllerTests
    {
        public static IEnumerable<object[]> PagesIndexRouteDataOk => new List<object[]>
        {
            new object[] { "/", nameof(PagesController.Index) },
            new object[] { "/pages", nameof(PagesController.Index) },
        };

        public static IEnumerable<object[]> PagesDocumentRouteDataOk => new List<object[]>
        {
            new object[] { "/pages/{documentId}", nameof(PagesController.Document) },
            new object[] { "/pages/{documentId}", nameof(PagesController.Document) },
        };

        [Theory]
        [MemberData(nameof(PagesIndexRouteDataOk))]
        public void PagesControllerCallsContentPageServiceUsingPagesIndexRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PagesDocumentRouteDataOk))]
        public void PagesControllerCallsContentPageServiceUsingPagesDocumentRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private static IActionResult RunControllerAction(PagesController controller, string actionName)
        {
            return actionName switch
            {
                nameof(PagesController.Document) => controller.Document(),
                _ => controller.Index(),
            };
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(Logger, FakeSessionStateService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}