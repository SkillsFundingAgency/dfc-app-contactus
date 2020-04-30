using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerPutTests : BasePagesController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerPutReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var existingModel = A.Fake<ContentPageModel>();
            existingModel.SequenceNumber = 123;

            var modelToPut = A.Fake<ContentPageModel>();
            modelToPut.SequenceNumber = 124;

            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);
            A.CallTo(() => FakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await controller.Update(modelToPut).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.UpsertAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            var okResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerPutReturnsAlreadyReportedForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.AlreadyReported;
            var existingModel = A.Fake<ContentPageModel>();
            existingModel.SequenceNumber = 123;

            var modelToUpsert = A.Fake<ContentPageModel>();
            modelToUpsert.SequenceNumber = existingModel.SequenceNumber - 1;

            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(existingModel);

            // Act
            var result = await controller.Update(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerPutReturnsNotFoundForUpdate(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.NotFound;
            ContentPageModel? expectedResult = null;
            var modelToUpsert = A.Fake<ContentPageModel>();

            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Update(modelToUpsert).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)expectedResponse, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerPutReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            ContentPageModel? contentPagesModel = null;
            var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.Update(contentPagesModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerPutReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var relatedCareersPagesModel = new ContentPageModel();
            var controller = BuildPagesController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Update(relatedCareersPagesModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)expectedResponse, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
