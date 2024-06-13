using DFC.App.ContactUs.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerBodyTests : BaseHomeControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task HomeControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored, A<double>.Ignored)).Returns(sharedHtmlResponse);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HomeControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored, A<double>.Ignored)).Returns(sharedHtmlResponse);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HomeControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored, A<double>.Ignored)).Returns(sharedHtmlResponse);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
