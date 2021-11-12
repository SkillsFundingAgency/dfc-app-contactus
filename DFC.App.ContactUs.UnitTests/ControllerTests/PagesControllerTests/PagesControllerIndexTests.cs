using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerIndexTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerIndexHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            A.Equals(3, model.Documents!.Count);
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerIndexJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);

            A.Equals(3, model.Documents!.Count);
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerIndexReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);
        }
    }
}
