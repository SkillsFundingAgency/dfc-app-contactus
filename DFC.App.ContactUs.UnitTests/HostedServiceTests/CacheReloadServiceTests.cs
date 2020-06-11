using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.HostedServices;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.HttpClientPolicies;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using DFC.App.ContactUs.Services.EventProcessorService.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.UnitTests.HostedServiceTests
{
    [Trait("Category", "Cache Reload background service Unit Tests")]
    public class CacheReloadServiceTests
    {
        private readonly ILogger<CacheReloadService> fakeLogger = A.Fake<ILogger<CacheReloadService>>();
        private readonly AutoMapper.IMapper fakeMapper = A.Fake<AutoMapper.IMapper>();
        private readonly CmsApiClientOptions fakeCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
        private readonly IEventMessageService<ContentPageModel> fakeEventMessageService = A.Fake<IEventMessageService<ContentPageModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();

        public CacheReloadServiceTests()
        {
            fakeCmsApiClientOptions.BaseAddress = new Uri("http://somewhere.com");
            fakeCmsApiClientOptions.SummaryEndpoint = "summary";
        }

        public static IEnumerable<object[]> TestValidationData => new List<object[]>
        {
            new object[] { BuildValidContentPageModel(), true },
            new object[] { A.Fake<ContentPageModel>(), false },
        };

        [Fact]
        public async Task CacheReloadServiceReloadIsSuccessfulForCreate()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(false);
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);
            var fakeCachedContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).Returns(fakeContactUsSummaryItemModels);
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(A.Fake<ContactUsApiDataModel>());
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.Created);
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).Returns(fakeCachedContentPageModels);
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceReloadIsSuccessfulForUpdate()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(false);
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);
            var fakeCachedContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).Returns(fakeContactUsSummaryItemModels);
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(A.Fake<ContactUsApiDataModel>());
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).Returns(fakeCachedContentPageModels);
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceReloadIsCancelled()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            var cancellationToken = new CancellationToken(true);
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);

            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).Returns(fakeContactUsSummaryItemModels);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsSuccessfulForCreate()
        {
            // arrange
            var cancellationToken = new CancellationToken(false);
            var expectedValidContactUsSummaryItemModel = BuildValidContactUsSummaryItemModel();
            var expectedValidContactUsApiDataModel = BuildValidContactUsApiDataModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidContactUsApiDataModel);
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.NotFound);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidContactUsSummaryItemModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsSuccessfulForUpdate()
        {
            // arrange
            var cancellationToken = new CancellationToken(false);
            var expectedValidContactUsSummaryItemModel = BuildValidContactUsSummaryItemModel();
            var expectedValidContactUsApiDataModel = BuildValidContactUsApiDataModel();
            var expectedValidContentPageModel = BuildValidContentPageModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidContactUsApiDataModel);
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidContactUsSummaryItemModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceGetAndSaveItemIsCancelled()
        {
            // arrange
            var cancellationToken = new CancellationToken(true);
            var expectedValidContactUsSummaryItemModel = BuildValidContactUsSummaryItemModel();
            var expectedValidContactUsApiDataModel = BuildValidContactUsApiDataModel();

            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).Returns(expectedValidContactUsApiDataModel);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.GetAndSaveItemAsync(expectedValidContactUsSummaryItemModel, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceDeleteStaleCacheEntriesIsSuccessful()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(false);
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);
            var fakeCachedContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).Returns(fakeCachedContentPageModels);
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.DeleteStaleCacheEntriesAsync(fakeContactUsSummaryItemModels, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceDeleteStaleCacheEntriesIsCancelled()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(true);
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);
            var fakeCachedContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).Returns(fakeCachedContentPageModels);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.DeleteStaleCacheEntriesAsync(fakeContactUsSummaryItemModels, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceDeleteStaleItemsIsSuccessful()
        {
            // arrange
            const int NumberOfDeletions = 2;
            var cancellationToken = new CancellationToken(false);
            var fakeContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.DeleteStaleItemsAsync(fakeContentPageModels, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceDeleteStaleItemsIsUnsuccessful()
        {
            // arrange
            const int NumberOfDeletions = 2;
            var cancellationToken = new CancellationToken(false);
            var fakeContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NotFound);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.DeleteStaleItemsAsync(fakeContentPageModels, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceDeleteStaleItemsIsCancelled()
        {
            // arrange
            const int NumberOfDeletions = 2;
            var cancellationToken = new CancellationToken(true);
            var fakeContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            await cacheReloadService.DeleteStaleItemsAsync(fakeContentPageModels, cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
        }

        [Theory]
        [MemberData(nameof(TestValidationData))]
        public void CacheReloadServiceTryValidateModelForValidAndInvalid(ContentPageModel contentPageModel, bool expectedResult)
        {
            // arrange
            var cacheReloadService = new CacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeCmsApiService);

            // act
            var result = cacheReloadService.TryValidateModel(contentPageModel);

            // assert
            A.Equals(result, expectedResult);
        }

        private static ContactUsSummaryItemModel BuildValidContactUsSummaryItemModel()
        {
            var model = new ContactUsSummaryItemModel()
            {
                CanonicalName = "an-article",
                Url = new Uri("/aaa/bbb", UriKind.Relative),
                Published = DateTime.UtcNow,
            };

            return model;
        }

        private static ContactUsApiDataModel BuildValidContactUsApiDataModel()
        {
            var model = new ContactUsApiDataModel()
            {
                ItemId = Guid.NewGuid(),
                CanonicalName = "an-article",
                Version = Guid.NewGuid(),
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Url = new Uri("/aaa/bbb", UriKind.Relative),
                AlternativeNames = new string[] { "alt-name-1", "alt-name-2" },
                Title = "A title",
                Description = "a description",
                Keywords = "some keywords",
                ContentItemUrls = new List<Uri> { new Uri("https://localhost/one"), new Uri("https://localhost/two"), new Uri("https://localhost/three"), },
                ContentItems = new List<ContactUsApiContentItemModel>
                {
                    new ContactUsApiContentItemModel { Row = 1, Column = 1, Width = 50, Content = "<h1>A document</h1>", },
                },
                Published = DateTime.UtcNow,
            };

            return model;
        }

        private static ContentPageModel BuildValidContentPageModel()
        {
            var model = new ContentPageModel()
            {
                Id = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("/aaa/bbb", UriKind.Relative),
                Content = "<h1>A document</h1>",
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        private List<ContactUsSummaryItemModel> BuldFakeContactUsSummaryItemModel(int iemCount)
        {
            var models = A.CollectionOfFake<ContactUsSummaryItemModel>(iemCount);

            foreach (var item in models)
            {
                item.CanonicalName = Guid.NewGuid().ToString();
            }

            return models.ToList();
        }

        private List<ContentPageModel> BuldFakeContentPageModels(int iemCount)
        {
            var models = A.CollectionOfFake<ContentPageModel>(iemCount);

            foreach (var item in models)
            {
                item.CanonicalName = Guid.NewGuid().ToString();
            }

            return models.ToList();
        }
    }
}
