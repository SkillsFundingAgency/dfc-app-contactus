using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using DFC.Compui.Sessionstate;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.EnterYourDetailsControllerTests
{
    [Trait("Category", "EnterYourDetails Controller Unit Tests")]
    public class EnterYourDetailsControllerBodyTests : BaseEnterYourDetailsControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task EnterYourDetailsControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<EnterYourDetailsBodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task EnterYourDetailsControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task EnterYourDetailsControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var fakeSessionStateModel = A.Fake<SessionStateModel<SessionDataModel>>();
            var controller = BuildEnterYourDetailsController(mediaTypeName);

            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).Returns(fakeSessionStateModel);

            // Act
            var result = await controller.EnterYourDetailsBody().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSessionStateService.GetAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
