using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var expectedConfigurationSetResults = A.Dummy<ConfigurationSetModel>();
            var expectedEmailResults = A.Dummy<EmailModel>();
            using var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedConfigurationSetResults);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedEmailResults);
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedTwiceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);

            A.Equals(3, model.Documents!.Count);
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerIndexJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedConfigurationSetResults = A.Dummy<ConfigurationSetModel>();
            var expectedEmailResults = A.Dummy<EmailModel>();
            using var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedConfigurationSetResults);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedEmailResults);
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedTwiceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(jsonResult.Value);

            A.Equals(3, model.Documents!.Count);
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerIndexReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var expectedConfigurationSetResults = A.Dummy<ConfigurationSetModel>();
            var expectedEmailResults = A.Dummy<EmailModel>();
            using var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedConfigurationSetResults);
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedEmailResults);
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).Returns(A.Fake<IndexDocumentViewModel>());

            // Act
            var result = await controller.Index().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<IndexDocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedTwiceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);
        }
    }
}
