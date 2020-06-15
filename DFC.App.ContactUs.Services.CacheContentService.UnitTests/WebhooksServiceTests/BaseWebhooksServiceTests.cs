﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

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
            FakeEventMessageService = A.Fake<IEventMessageService<ContentPageModel>>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeContentPageService = A.Fake<IContentPageService<ContentPageModel>>();
            FakeContentCacheService = A.Fake<IContentCacheService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentItemIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService> Logger { get; }

        protected IEventMessageService<ContentPageModel> FakeEventMessageService { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IContentPageService<ContentPageModel> FakeContentPageService { get; }

        protected IContentCacheService FakeContentCacheService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected static ContactUsApiDataModel BuildValidContactUsApiContentModel()
        {
            var model = new ContactUsApiDataModel
            {
                ItemId = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                ContentItemUrls = new List<Uri> { new Uri("https://localhost/one"), new Uri("https://localhost/two"), new Uri("https://localhost/three"), },
                ContentItems = new List<ContactUsApiContentItemModel>
                {
                    BuildValidContactUsApiContentItemDataModel(),
                },
                Published = DateTime.UtcNow,
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

        protected ContentPageModel BuildValidContentPageModel()
        {
            var model = new ContentPageModel()
            {
                Id = ContentIdForUpdate,
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                Content = null,
                ContentItems = new List<ContentItemModel>
                {
                    BuildValidContentItemModel(ContentItemIdForCreate),
                    BuildValidContentItemModel(ContentItemIdForUpdate),
                    BuildValidContentItemModel(ContentItemIdForDelete),
                },
                LastReviewed = DateTime.UtcNow,
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
            var service = new WebhooksService(Logger, FakeMapper, FakeEventMessageService, FakeCmsApiService, FakeContentPageService, FakeContentCacheService);

            return service;
        }
    }
}