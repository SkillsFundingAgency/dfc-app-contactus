using AutoMapper;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    public class ConfigurationSetReloadServiceTests
    {
        private readonly IMapper fakeMapper = A.Fake<IMapper>();
        private readonly IDocumentService<ConfigurationSetModel> fakeConfigurationSetDocumentService = A.Fake<IDocumentService<ConfigurationSetModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();
        private readonly IContentTypeMappingService fakeContentTypeMappingService = A.Fake<IContentTypeMappingService>();

        [Fact]
        public async Task ConfigurationSetReloadServiceReloadCancellationRequestedCancels()
        {
            //Arrange
            var dummyCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
            var cancellationToken = new CancellationToken(true);
            var configurationSetReloadService = new ConfigurationSetReloadService(A.Fake<ILogger<ConfigurationSetReloadService>>(), fakeMapper, fakeConfigurationSetDocumentService, fakeCmsApiService, dummyCmsApiClientOptions, fakeContentTypeMappingService);

            //Act
            await configurationSetReloadService.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ConfigurationSetReloadServiceIsMissingApiResponse()
        {
            //Arrange
            var dummyCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
            ConfigurationSetApiDataModel? nullConfigurationSetApiDataModel = null;

            dummyCmsApiClientOptions.BaseAddress = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(nullConfigurationSetApiDataModel);

            var configurationSetReloadService = new ConfigurationSetReloadService(A.Fake<ILogger<ConfigurationSetReloadService>>(), fakeMapper, fakeConfigurationSetDocumentService, fakeCmsApiService, dummyCmsApiClientOptions, fakeContentTypeMappingService);

            //Act
            await configurationSetReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ConfigurationSetReloadServiceIsMissingContentItems()
        {
            //Arrange
            var dummyCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
            var fakeConfigurationSetApiDataModel = A.Dummy<ConfigurationSetApiDataModel>();

            dummyCmsApiClientOptions.BaseAddress = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(fakeConfigurationSetApiDataModel);

            var configurationSetReloadService = new ConfigurationSetReloadService(A.Fake<ILogger<ConfigurationSetReloadService>>(), fakeMapper, fakeConfigurationSetDocumentService, fakeCmsApiService, dummyCmsApiClientOptions, fakeContentTypeMappingService);

            //Act
            await configurationSetReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task ConfigurationSetReloadServiceReloadReloadsItems()
        {
            //Arrange
            var dummyCmsApiClientOptions = A.Dummy<CmsApiClientOptions>();
            var fakeConfigurationSetApiDataModel = A.Dummy<ConfigurationSetApiDataModel>();
            var fakeConfigurationSetModel = A.Dummy<ConfigurationSetModel>();

            fakeConfigurationSetApiDataModel.ContentItems = A.CollectionOfDummy<IBaseContentItemModel>(2);
            dummyCmsApiClientOptions.BaseAddress = new Uri("https://www.somewhere.com", UriKind.Absolute);

            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).Returns(fakeConfigurationSetApiDataModel);
            A.CallTo(() => fakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetModel>.Ignored)).Returns(fakeConfigurationSetModel);

            var configurationSetReloadService = new ConfigurationSetReloadService(A.Fake<ILogger<ConfigurationSetReloadService>>(), fakeMapper, fakeConfigurationSetDocumentService, fakeCmsApiService, dummyCmsApiClientOptions, fakeContentTypeMappingService);

            //Act
            await configurationSetReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<ConfigurationSetApiDataModel>(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeMapper.Map<ConfigurationSetModel>(A<ConfigurationSetApiDataModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeConfigurationSetDocumentService.UpsertAsync(A<ConfigurationSetModel>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
