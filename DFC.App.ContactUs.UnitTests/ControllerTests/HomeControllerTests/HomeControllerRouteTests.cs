using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerRouteTests : BaseHomeControllerTests
    {
        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/{LocalPath}/{HomeController.ThisViewCanonicalName}", nameof(HomeController.HomeView) },
            new object[] { $"/{LocalPath}/{HomeController.ThisViewCanonicalName}/htmlhead", nameof(HomeController.HomeHtmlHead) },
            new object[] { $"/{LocalPath}/{HomeController.ThisViewCanonicalName}/breadcrumb", nameof(HomeController.HomeBreadcrumb) },
            new object[] { $"/{LocalPath}/{HomeController.ThisViewCanonicalName}/body", nameof(HomeController.HomeBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public void PagesControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private IActionResult RunControllerAction(HomeController controller, string actionName)
        {
            return actionName switch
            {
                nameof(HomeController.HomeHtmlHead) => controller.HomeHtmlHead(),
                nameof(HomeController.HomeBreadcrumb) => controller.HomeBreadcrumb(),
                _ => controller.HomeBody(),
            };
        }

        private HomeController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new HomeController(Logger, FakeSessionStateService, FakeServiceOpenDetailModel)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}