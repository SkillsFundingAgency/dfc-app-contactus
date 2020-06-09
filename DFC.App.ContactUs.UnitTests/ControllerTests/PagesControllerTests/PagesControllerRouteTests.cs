using AutoMapper;
using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerRouteTests
    {
        private readonly ILogger<PagesController> logger;
        private readonly IContentPageService<ContentPageModel> fakeContentPageService;
        private readonly IMapper fakeMapper;

        public PagesControllerRouteTests()
        {
            logger = A.Fake<ILogger<PagesController>>();
            fakeContentPageService = A.Fake<IContentPageService<ContentPageModel>>();
            fakeMapper = A.Fake<IMapper>();
        }

        public static IEnumerable<object[]> PagesRouteDataOk => new List<object[]>
        {
            new object[] { "/", string.Empty, nameof(PagesController.Index) },
            new object[] { "/pages", string.Empty, nameof(PagesController.Index) },
            new object[] { "/pages/{article}", "SomeArticle", nameof(PagesController.Document) },
            new object[] { "/pages/{article}/htmlhead", "SomeArticle", nameof(PagesController.HtmlHead) },
            new object[] { "/pages/htmlhead", string.Empty, nameof(PagesController.HtmlHead) },
            new object[] { "/pages/{article}/breadcrumb", "SomeArticle", nameof(PagesController.Breadcrumb) },
            new object[] { "/pages/breadcrumb", string.Empty, nameof(PagesController.Breadcrumb) },
            new object[] { "/pages/{article}/body", "SomeArticle", nameof(PagesController.Body) },
            new object[] { "/pages/body", string.Empty, nameof(PagesController.Body) },
        };

        public static IEnumerable<object[]> PagesRouteDataNoContent => new List<object[]>
        {
            new object[] { "/pages/{article}/bodytop", "SomeArticle", nameof(PagesController.BodyTop) },
            new object[] { "/pages/bodytop", string.Empty, nameof(PagesController.BodyTop) },
            new object[] { "/pages/{article}/herobanner", "SomeArticle", nameof(PagesController.HeroBanner) },
            new object[] { "/pages/herobanner", string.Empty, nameof(PagesController.HeroBanner) },
            new object[] { "/pages/{article}/sidebarright", "SomeArticle", nameof(PagesController.SidebarRight) },
            new object[] { "/pages/sidebarright", string.Empty, nameof(PagesController.SidebarRight) },
            new object[] { "/pages/{article}/sidebarleft", "SomeArticle", nameof(PagesController.SidebarLeft) },
            new object[] { "/pages/sidebarleft", string.Empty, nameof(PagesController.SidebarLeft) },
            new object[] { "/pages/{article}/bodyfooter", "SomeArticle", nameof(PagesController.BodyFooter) },
            new object[] { "/pages/bodyfooter", string.Empty, nameof(PagesController.BodyFooter) },
        };

        [Theory]
        [MemberData(nameof(PagesRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRouteForOkResult(string route, string article, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);
            var expectedResult = new ContentPageModel() { Content = "<h1>A document</h1>" };

            A.CallTo(() => fakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await RunControllerAction(controller, article, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => fakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(PagesRouteDataNoContent))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRouteFornoContentResult(string route, string article, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);

            // Act
            var result = await RunControllerAction(controller, article, actionMethod).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, string article, string actionName)
        {
            return actionName switch
            {
                nameof(PagesController.HtmlHead) => await controller.HtmlHead(article).ConfigureAwait(false),
                nameof(PagesController.Breadcrumb) => await controller.Breadcrumb(article).ConfigureAwait(false),
                nameof(PagesController.BodyTop) => controller.BodyTop(article),
                nameof(PagesController.HeroBanner) => controller.HeroBanner(article),
                nameof(PagesController.SidebarRight) => controller.SidebarRight(article),
                nameof(PagesController.SidebarLeft) => controller.SidebarLeft(article),
                nameof(PagesController.BodyFooter) => controller.BodyFooter(article),
                _ => await controller.Body(article).ConfigureAwait(false),
            };
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(logger, fakeContentPageService, fakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}