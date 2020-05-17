using DFC.App.ContactUs.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerRouteTests
    {
        private readonly ILogger<EnterYourDetailsController> logger;

        public EnterYourDetailsControllerRouteTests()
        {
            logger = A.Fake<ILogger<EnterYourDetailsController>>();
        }

        public static IEnumerable<object[]> RouteDataOk => new List<object[]>
        {
            new object[] { $"/pages/{EnterYourDetailsController.ThisViewCanonicalName}",  nameof(EnterYourDetailsController.EnterYourDetailsView) },
            new object[] { $"/pages/{EnterYourDetailsController.ThisViewCanonicalName}/htmlhead",  nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) },
            new object[] { $"/pages/{EnterYourDetailsController.ThisViewCanonicalName}/breadcrumb", nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) },
            new object[] { $"/pages/{EnterYourDetailsController.ThisViewCanonicalName}/body", nameof(EnterYourDetailsController.EnterYourDetailsBody) },
        };

        [Theory]
        [MemberData(nameof(RouteDataOk))]
        public void EnterYourDetailsControllerUsingPagesViewRouteForOkResult(string route, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = RunControllerAction(controller, actionMethod);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            controller.Dispose();
        }

        private IActionResult RunControllerAction(EnterYourDetailsController controller, string actionName)
        {
            return actionName switch
            {
                nameof(EnterYourDetailsController.EnterYourDetailsHtmlHead) => controller.EnterYourDetailsHtmlHead(),
                nameof(EnterYourDetailsController.EnterYourDetailsBreadcrumb) => controller.EnterYourDetailsBreadcrumb(),
                _ => controller.EnterYourDetailsBody(),
            };
        }

        private EnterYourDetailsController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new EnterYourDetailsController(logger)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}