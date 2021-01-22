using DFC.App.ContactUs.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerRouteTests : BaseEnterYourDetailsControllerTests
    {
        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}",  nameof(EnterYourDetailsController.EnterYourDetailsView) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/htmlhead",  nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/breadcrumb", nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) },
            new object[] { $"/{LocalPath}/{EnterYourDetailsController.ThisViewCanonicalName}/body", nameof(EnterYourDetailsController.EnterYourDetailsBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public async Task EnterYourDetailsControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerActionAsync(controller, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private async Task<IActionResult> RunControllerActionAsync(EnterYourDetailsController controller, string actionName)
        {
            return actionName switch
            {
                nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) => controller.EnterYourDetailsHtmlHead(),
                nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) => controller.EnterYourDetailsBreadcrumb(),
                _ => await controller.EnterYourDetailsBody().ConfigureAwait(false),
            };
        }

        private EnterYourDetailsController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new EnterYourDetailsController(Logger, FakeMapper, FakeSessionStateService, FakeRoutingService, FakeNotifyEmailService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}