using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
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
        public async Task PagesControllerCallsContentPageServiceUsingPagesIndexRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PagesDocumentRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesDocumentRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, string actionName)
        {
            return actionName switch
            {
                nameof(PagesController.Document) => await controller.Document().ConfigureAwait(false),
                _ => await controller.Index().ConfigureAwait(false),
            };
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(Logger, FakeSessionStateService, FakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}