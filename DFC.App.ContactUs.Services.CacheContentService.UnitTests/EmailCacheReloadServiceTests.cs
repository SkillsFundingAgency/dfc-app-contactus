using AutoMapper;
using Castle.Core.Internal;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    public class EmailCacheReloadServiceTests
    {
        private readonly IApiDataProcessorService apiDataProcessorService = A.Fake<IApiDataProcessorService>();
        private readonly IDocumentService<EmailModel> fakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();
        private readonly IMapper fakeMapper = A.Fake<IMapper>();

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllCancellationRequestedCancels()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var emailReloadService = new EmailCacheReloadService(apiDataProcessorService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper, A.Fake<HttpClient>());

            //Act
            await emailReloadService.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => apiDataProcessorService.GetAsync<EmailApiDataModel>(A<HttpClient>.Ignored, A<string>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllReloadsItems()
        {
            //Arrange
            var fakeEmails = new List<EmailApiDataModel>();
            foreach (var key in EmailKeyHelper.GetEmailKeys())
            {
                fakeEmails.Add(new EmailApiDataModel { Body = "<h1>Test</h1>", Url = new Uri($"http://somehost.com/{key}") });
            }

            A.CallTo(() => apiDataProcessorService.GetAsync<List<EmailApiDataModel>>(A<HttpClient>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(fakeEmails);
            var emailReloadService = new EmailCacheReloadService(apiDataProcessorService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper, A.Fake<HttpClient>());

            //Act
            await emailReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => apiDataProcessorService.GetAsync<EmailApiDataModel>(A<HttpClient>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappened(EmailKeyHelper.GetEmailKeys().Count(), Times.Exactly);
        }

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllReloadSingleItem()
        {
            //Arrange
            var fakeEmail = new EmailApiDataModel { Body = "<h1>Test</h1>", Url = new Uri($"http://somehost.com/9f4b6845-2f6f-43e8-a6c3-fb5a4cc3fd31") };

            A.CallTo(() => apiDataProcessorService.GetAsync<EmailApiDataModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).Returns(fakeEmail);
            var emailReloadService = new EmailCacheReloadService(apiDataProcessorService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper, A.Fake<HttpClient>());

            //Act
            await emailReloadService.ReloadCacheItem(new Uri($"http://somehost.com/9f4b6845-2f6f-43e8-a6c3-fb5a4cc3fd31")).ConfigureAwait(false);

            //Assert
            A.CallTo(() => apiDataProcessorService.GetAsync<EmailApiDataModel>(A<HttpClient>.Ignored, A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
