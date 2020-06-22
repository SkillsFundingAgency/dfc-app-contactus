using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    [Trait("Category", "Cache Reload background service Unit Tests")]
    public class CacheReloadServiceTests
    {
        private readonly ILogger<ContentPageModelCacheReloadService> fakeLogger = A.Fake<ILogger<ContentPageModelCacheReloadService>>();
        private readonly AutoMapper.IMapper fakeMapper = A.Fake<AutoMapper.IMapper>();
        private readonly CmsApiClientOptions fakeCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
        private readonly IEventMessageService<ContentPageModel> fakeEventMessageService = A.Fake<IEventMessageService<ContentPageModel>>();
        private readonly IContentApiService<ContactUsSummaryItemModel> fakeContentApiService = A.Fake<IContentApiService<ContactUsSummaryItemModel>>();
        private readonly IContentCacheService fakeContentCacheService = A.Fake<IContentCacheService>();

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
        public async Task CacheReloadServiceReloadIsSuccessfulForUpdate()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            const int NumberOfDeletions = 3;
            var cancellationToken = new CancellationToken(false);
            var expectedValidContentPageModel = BuildValidContentPageModel();
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);
            var fakeCachedContentPageModels = BuldFakeContentPageModels(NumberOfDeletions);

            A.CallTo(() => fakeContentCacheService.Clear());
            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).Returns(fakeContactUsSummaryItemModels);
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).Returns(expectedValidContentPageModel);
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).Returns(HttpStatusCode.OK);
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).Returns(fakeCachedContentPageModels);
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);
            A.CallTo(() => fakeContentCacheService.AddOrReplace(A<Guid>.Ignored, A<List<Guid>>.Ignored));

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeContentCacheService.Clear()).MustHaveHappenedOnceExactly();
            ////A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            //A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
            //A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened(NumberOfDeletions, Times.Exactly);
            //A.CallTo(() => fakeContentCacheService.AddOrReplace(A<Guid>.Ignored, A<List<Guid>>.Ignored)).MustHaveHappened(NumerOfSummaryItems, Times.Exactly);
        }

        [Fact]
        public async Task CacheReloadServiceReloadIsCancelled()
        {
            // arrange
            const int NumerOfSummaryItems = 2;
            var cancellationToken = new CancellationToken(true);
            var fakeContactUsSummaryItemModels = BuldFakeContactUsSummaryItemModel(NumerOfSummaryItems);

            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).Returns(fakeContactUsSummaryItemModels);

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

            // act
            await cacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            // assert
            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeContentCacheService.Clear()).MustNotHaveHappened();
            A.CallTo(() => fakeMapper.Map<ContentPageModel>(A<ContactUsApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.UpdateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.CreateAsync(A<ContentPageModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.GetAllCachedCanonicalNamesAsync()).MustNotHaveHappened();
            A.CallTo(() => fakeEventMessageService.DeleteAsync(A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeContentCacheService.AddOrReplace(A<Guid>.Ignored, A<List<Guid>>.Ignored)).MustNotHaveHappened();
        }

        private IContentApiService<ContactUsSummaryItemModel> BuildFakeContentApiService()
        {
            return A.Fake<IContentApiService<ContactUsSummaryItemModel>>();
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

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

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

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

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

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

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

            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

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
            var cacheReloadService = new ContentPageModelCacheReloadService(fakeLogger, fakeMapper, fakeEventMessageService, fakeContentApiService, fakeContentCacheService);

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
                    new ContactUsApiContentItemModel { Justify = 1, Ordinal = 1, Width = 50, Content = "<h1>A document</h1>", },
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
                ContentItems = new List<ContentItemModel>
                {
                    new ContentItemModel { Justify = 1, Ordinal = 1, Width = 50, Content = "<h1>A document</h1>", },
                },
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
