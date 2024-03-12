using DFC.App.ContactUs.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerHomeViewTests : BaseHomeControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task HomeControllerViewHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored)).Returns(sharedHtmlResponse);


            // Act
            var result = await controller.HomeView().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HomeControllerViewJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored)).Returns(sharedHtmlResponse);

            // Act
            var result = await controller.HomeView().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<HomeViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HomeControllerHomeViewReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            var sharedHtmlResponse = new SharedHtml()
            {
                Html = "Test",
            };

            var test = A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<SharedHtml>("test", A<string>.Ignored)).Returns(sharedHtmlResponse);

            // Act
            var result = await controller.HomeView().ConfigureAwait(false);

            // Assert
            Assert.NotNull(test);

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
