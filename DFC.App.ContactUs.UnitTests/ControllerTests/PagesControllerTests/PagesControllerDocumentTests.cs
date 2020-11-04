using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
<<<<<<< HEAD
using FluentAssertions;
=======
>>>>>>> story/DFCC-1169-refresh-nugets
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerDocumentTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
<<<<<<< HEAD
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);
            var expectedModel = new DocumentViewModel
            {
                DocumentId = Guid.NewGuid(),
                CanonicalName = "a-canonical-name",
                PartitionKey = "partition-key",
                Version = Guid.NewGuid(),
                BreadcrumbTitle = "Title",
                IncludeInSitemap = true,
                Url = new Uri("https://somewhere.com", UriKind.Absolute),
                Content = new Microsoft.AspNetCore.Html.HtmlString("some content"),
                LastReviewed = DateTime.Now,
            };
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(expectedModel);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
=======
            var expectedResult = A.Fake<EmailModel>();
            var controller = BuildPagesController(mediaTypeName);
            var expectedModel = new DocumentViewModel
            {
                Id = Guid.NewGuid(),
                Title = "A title",
                PartitionKey = "partition-key",
                Url = new Uri("https://somewhere.com", UriKind.Absolute),
                BodyViewModel = new BodyViewModel { Body = new Microsoft.AspNetCore.Html.HtmlString("some content") },
                LastReviewed = DateTime.Now,
            };
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).Returns(expectedModel);

            // Act
            var result = await controller.Document(expectedModel.Id).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
>>>>>>> story/DFCC-1169-refresh-nugets

            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<DocumentViewModel>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as DocumentViewModel;
            Assert.Equal(expectedModel, model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerDocumentJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
<<<<<<< HEAD
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
=======
            var expectedResult = A.Fake<EmailModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
>>>>>>> story/DFCC-1169-refresh-nugets

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
<<<<<<< HEAD
        [MemberData(nameof(HtmlMediaTypes))]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task PagesControllerDocumentReturnsRedirectWhenAlternateArticleExists(string mediaTypeName)
        {
            // Arrange
            const string article = "an-article-name";
            ContentPageModel? expectedResult = null;
            var expectedAlternativeResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            expectedAlternativeResult.CanonicalName = article;

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeContentPageService.GetByRedirectLocationAsync(A<string>.Ignored)).Returns(expectedAlternativeResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByRedirectLocationAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<RedirectResult>(result);

            statusResult.Url.Should().NotBeNullOrWhiteSpace();
            A.Equals(true, statusResult.Permanent);

            controller.Dispose();
        }

        [Theory]
=======
>>>>>>> story/DFCC-1169-refresh-nugets
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
<<<<<<< HEAD
            const string article = "an-article-name";
            ContentPageModel? expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeContentPageService.GetByRedirectLocationAsync(A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeContentPageService.GetByRedirectLocationAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
=======
            EmailModel? expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
>>>>>>> story/DFCC-1169-refresh-nugets

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerDocumentReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
<<<<<<< HEAD
            const string article = "an-article-name";
            var expectedResult = A.Fake<ContentPageModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(article).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeContentPageService.GetByNameAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
=======
            var expectedResult = A.Fake<EmailModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeEmailDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
>>>>>>> story/DFCC-1169-refresh-nugets

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
