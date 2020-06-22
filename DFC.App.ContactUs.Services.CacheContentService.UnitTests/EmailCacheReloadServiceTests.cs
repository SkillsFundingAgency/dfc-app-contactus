using AutoMapper;
using Castle.Core.Internal;
using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    public class EmailCacheReloadServiceTests
    {
        private readonly IContentApiService<EmailApiDataModel> fakeContentApiService = A.Fake<IContentApiService<EmailApiDataModel>>();
        private readonly IDocumentService<EmailModel> fakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();
        private readonly IMapper fakeMapper = A.Fake<IMapper>();

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllCancellationRequestedCancels()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var emailReloadService = new EmailCacheReloadService(fakeContentApiService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper);

            //Act
            await emailReloadService.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).MustNotHaveHappened();
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

            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).Returns(fakeEmails);
            var emailReloadService = new EmailCacheReloadService(fakeContentApiService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper);

            //Act
            await emailReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeContentApiService.GetAll(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappened(EmailKeyHelper.GetEmailKeys().Count(), Times.Exactly);
        }

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllReloadSingleItem()
        {
            //Arrange
            var fakeEmails = new List<EmailApiDataModel>();
            foreach (var key in EmailKeyHelper.GetEmailKeys())
            {
                fakeEmails.Add(new EmailApiDataModel { Body = "<h1>Test</h1>", Url = new Uri($"http://somehost.com/{key}") });
            }

            A.CallTo(() => fakeContentApiService.GetById(A<Uri>.Ignored)).Returns(fakeEmails.FirstOrDefault());
            var emailReloadService = new EmailCacheReloadService(fakeContentApiService, A.Fake<ILogger<EmailCacheReloadService>>(), fakeEmailDocumentService, fakeMapper);

            //Act
            await emailReloadService.ReloadCacheItem(fakeEmails.FirstOrDefault().Url).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeContentApiService.GetById(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
