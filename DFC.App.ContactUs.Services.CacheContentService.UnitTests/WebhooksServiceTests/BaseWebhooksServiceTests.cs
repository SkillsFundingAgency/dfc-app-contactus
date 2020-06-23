using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
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
            FakeEmailEventMessageService = A.Fake<IEventMessageService<EmailModel>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeEmailCacheReloadService = A.Fake<IEmailCacheReloadService>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService> Logger { get; }

        protected IEventMessageService<EmailModel> FakeEmailEventMessageService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected IEmailCacheReloadService FakeEmailCacheReloadService { get; }

        protected static EmailApiDataModel BuildValidEmailApiModel()
        {
            var model = new EmailApiDataModel
            {
                Body = "<h1>Test</h1>",
            };

            return model;
        }

        protected static ContactUsApiContentItemModel BuildValidContactUsApiContentItemDataModel()
        {
            var model = new ContactUsApiContentItemModel
            {
                Justify = 1,
                Ordinal = 1,
                Width = 50,
                Content = "<h1>A document</h1>",
            };

            return model;
        }

        protected EmailModel BuildValidEmailModel()
        {
            var model = new EmailModel()
            {
                Id = ContentIdForUpdate,
                Body = "<h1>Test</h1>",
            };

            return model;
        }

        protected ContentItemModel BuildValidContentItemModel(Guid contentItemId)
        {
            var model = new ContentItemModel()
            {
                ItemId = contentItemId,
                Version = Guid.NewGuid(),
                LastReviewed = DateTime.Now,
            };

            return model;
        }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(Logger, FakeEmailEventMessageService, FakeEmailCacheReloadService);

            return service;
        }
    }
}