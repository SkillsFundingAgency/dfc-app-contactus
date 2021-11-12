using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
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
            new object[] { "/", Guid.Empty, nameof(PagesController.Index) },
            new object[] { "/pages", Guid.Empty, nameof(PagesController.Index) },
        };

        public static IEnumerable<object[]> PagesDocumentRouteDataOk => new List<object[]>
        {
            new object[] { "/pages/{documentId}", ConfigurationSetKeyHelper.ConfigurationSetKey, nameof(PagesController.Document), 1 },
            new object[] { "/pages/{documentId}", Guid.NewGuid(), nameof(PagesController.Document), 1 },
        };

        [Theory]
        [MemberData(nameof(PagesIndexRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesIndexRouteForOkResult(string route, Guid documentId, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);
            var expectedConfigurationSetResult = new ConfigurationSetModel() { PhoneNumber = "1234", LinesOpenText = "lines are open" };

            // Act
            var result = await RunControllerAction(controller, documentId, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PagesDocumentRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesDocumentRouteForOkResult(string route, Guid documentId, string actionMethod, int configurationSetCount)
        {
            // Arrange
            var controller = BuildController(route);
            var expectedConfigurationSetResult = new ConfigurationSetModel() { PhoneNumber = "1234", LinesOpenText = "lines are open" };

            // Act
            var result = await RunControllerAction(controller, documentId, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, Guid documentId, string actionName)
        {
            return actionName switch
            {
                nameof(PagesController.Document) => await controller.Document(documentId).ConfigureAwait(false),
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