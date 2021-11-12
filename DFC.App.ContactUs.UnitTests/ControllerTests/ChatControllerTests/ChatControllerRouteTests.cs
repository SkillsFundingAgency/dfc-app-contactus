using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.ChatControllerTests
{
    [Trait("Category", "Chat Controller Unit Tests")]
    public class ChatControllerRouteTests : BaseChatControllerTests
    {
        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/pages/{ChatController.ThisViewCanonicalName}",  nameof(ChatController.ChatView) },
            new object[] { $"/pages/{ChatController.ThisViewCanonicalName}/htmlhead",  nameof(ChatController.ChatHtmlHead) },
            new object[] { $"/pages/{ChatController.ThisViewCanonicalName}/breadcrumb",  nameof(ChatController.ChatBreadcrumb) },
            new object[] { $"/pages/{ChatController.ThisViewCanonicalName}/body",  nameof(ChatController.ChatBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public async Task ChatControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private async Task<IActionResult> RunControllerAction(ChatController controller, string actionName)
        {
            return actionName switch
            {
                nameof(ChatController.ChatHtmlHead) => controller.ChatHtmlHead(),
                nameof(ChatController.ChatBreadcrumb) => controller.ChatBreadcrumb(),
                _ => await controller.ChatBody().ConfigureAwait(false),
            };
        }

        private ChatController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new ChatController(Logger, FakeSessionStateService, ChatOptions, FakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}