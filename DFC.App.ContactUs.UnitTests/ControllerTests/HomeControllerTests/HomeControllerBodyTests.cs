using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Category", "Home Controller Unit Tests")]
    public class HomeControllerBodyTests : BaseHomeControllerTests
    {
        /*[Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task HomeControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<HomeBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }*/

        /*[Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task HomeControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }*/

        /*[Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task HomeControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).Returns(true);

            // Act
            var result = await controller.HomeBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }*/
    }
}
