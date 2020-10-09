using DFC.App.ContactUs.Controllers;
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
        public static IEnumerable<object[]> PagesRouteDataOk => new List<object[]>
        {
            new object[] { "/", Guid.Empty, nameof(PagesController.Index) },
            new object[] { "/pages", Guid.Empty, nameof(PagesController.Index) },
            new object[] { "/pages/{documentId}", Guid.NewGuid(), nameof(PagesController.Document) },
        };

        [Theory]
        [MemberData(nameof(PagesRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRouteForOkResult(string route, Guid documentId, string actionMethod)
        {
            // Arrange
            var controller = BuildController(route);
            var expectedResult = new EmailModel() { Body = "<h1>A document</h1>" };
            var expectedResults = new List<EmailModel> { expectedResult };

            A.CallTo(() => FakeEmailDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await RunControllerAction(controller, documentId, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            A.CallTo(() => FakeEmailDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceOrLess();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceOrLess();

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

            return new PagesController(Logger, FakeSessionStateService, FakeEmailDocumentService, FakeMapper)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}