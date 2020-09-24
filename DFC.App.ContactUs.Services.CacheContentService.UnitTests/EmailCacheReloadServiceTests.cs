using AutoMapper;
using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests
{
    public class EmailCacheReloadServiceTests
    {
        private readonly IMapper fakeMapper = A.Fake<IMapper>();
        private readonly IDocumentService<EmailModel> fakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllCancellationRequestedCancels()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var emailReloadService = new EmailCacheReloadService(A.Fake<ILogger<EmailCacheReloadService>>(), fakeMapper, fakeEmailDocumentService, fakeCmsApiService);

            //Act
            await emailReloadService.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task EmailCacheReloadServiceReloadAllReloadsItems()
        {
            //Arrange
            var fakeEmail = A.Dummy<EmailApiDataModel>();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(fakeEmail);
            var emailReloadService = new EmailCacheReloadService(A.Fake<ILogger<EmailCacheReloadService>>(), fakeMapper, fakeEmailDocumentService, fakeCmsApiService);

            //Act
            await emailReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<EmailApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).MustHaveHappened(EmailKeyHelper.GetEmailKeys().Count(), Times.Exactly);
            A.CallTo(() => fakeEmailDocumentService.UpsertAsync(A<EmailModel>.Ignored)).MustHaveHappened(EmailKeyHelper.GetEmailKeys().Count(), Times.Exactly);
        }
    }
}
