using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerRouteTests
    {
        private readonly ILogger<HomeController> logger;
        private readonly ServiceOpenDetailModel fakeServiceOpenDetailModel;

        public HomeControllerRouteTests()
        {
            logger = A.Fake<ILogger<HomeController>>();
            fakeServiceOpenDetailModel = A.Fake<ServiceOpenDetailModel>();
        }

        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/pages/{HomeController.ThisViewCanonicalName}", nameof(HomeController.HomeView) },
            new object[] { $"/pages/{HomeController.ThisViewCanonicalName}/htmlhead", nameof(HomeController.HomeHtmlHead) },
            new object[] { $"/pages/{HomeController.ThisViewCanonicalName}/breadcrumb", nameof(HomeController.HomeBreadcrumb) },
            new object[] { $"/pages/{HomeController.ThisViewCanonicalName}/body", nameof(HomeController.HomeBody) },
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

            return new HomeController(logger, fakeServiceOpenDetailModel)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}