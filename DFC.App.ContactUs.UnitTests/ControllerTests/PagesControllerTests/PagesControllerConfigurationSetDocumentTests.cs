using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerConfigurationSetDocumentTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<ConfigurationSetModel>();
            var controller = BuildPagesController(mediaTypeName);
            var expectedModel = new DocumentViewModel
            {
                Id = ConfigurationSetKeyHelper.ConfigurationSetKey,
                Title = "A title",
                PartitionKey = "partition-key",
                Url = new Uri("https://somewhere.com", UriKind.Absolute),
                ConfigurationSetBodyViewModel = new ConfigurationSetBodyViewModel { PhoneNumber = "1234", LinesOpenText = "lines are open" },
                LastReviewed = DateTime.Now,
            };
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(expectedModel);

            // Act
            var result = await controller.Document(expectedModel.Id).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();

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
            var expectedResult = A.Fake<ConfigurationSetModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            _ = Assert.IsAssignableFrom<DocumentViewModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerDocumentReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            ConfigurationSetModel? expectedResult = null;
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Document(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task PagesControllerDocumentReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var expectedResult = A.Fake<ConfigurationSetModel>();
            var controller = BuildPagesController(mediaTypeName);

            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).Returns(A.Fake<DocumentViewModel>());

            // Act
            var result = await controller.Document(ConfigurationSetKeyHelper.ConfigurationSetKey).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeConfigurationSetDocumentService.GetByIdAsync(A<Guid>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<DocumentViewModel>(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
