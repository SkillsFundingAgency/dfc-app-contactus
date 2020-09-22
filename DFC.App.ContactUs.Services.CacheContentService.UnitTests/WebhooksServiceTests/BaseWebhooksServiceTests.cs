using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    public abstract class BaseWebhooksServiceTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected BaseWebhooksServiceTests()
        {
            Logger = A.Fake<ILogger<WebhooksService>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeEmailDocumentService = A.Fake<IDocumentService<EmailModel>>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService> Logger { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IDocumentService<EmailModel> FakeEmailDocumentService { get; }

        protected static EmailApiDataModel BuildValidEmailApiDataModel()
        {
            var model = new EmailApiDataModel
            {
                Title = "an-article",
                Url = new Uri("https://localhost"),
                To = "to-address",
                From = "from-address",
                Subject = "the subject",
                Body = "some body test",
            };

            return model;
        }

        protected EmailModel BuildValidEmailModel()
        {
            var model = new EmailModel()
            {
                Id = ContentIdForUpdate,
                Etag = Guid.NewGuid().ToString(),
                Title = "an-article",
                Url = new Uri("https://localhost"),
                Body = "some body test",
                LastCached = DateTime.UtcNow,
            };

            return model;
        }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(Logger, FakeMapper, FakeCmsApiService, FakeEmailDocumentService);

            return service;
        }
    }
}
