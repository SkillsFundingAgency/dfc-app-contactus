using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
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
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeConfigurationSetDocumentService = A.Fake<IDocumentService<ConfigurationSetModel>>();
        }

        protected Guid ContentIdForEmailCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForEmailUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForEmailDelete { get; } = Guid.NewGuid();

        protected Guid ContentIdForConfigurationSetCreate { get; } = ConfigurationSetKeyHelper.ConfigurationSetKey;

        protected Guid ContentIdForConfigurationSetUpdate { get; } = ConfigurationSetKeyHelper.ConfigurationSetKey;

        protected Guid ContentIdForConfigurationSetDelete { get; } = ConfigurationSetKeyHelper.ConfigurationSetKey;

        protected ILogger<WebhooksService> Logger { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IDocumentService<ConfigurationSetModel> FakeConfigurationSetDocumentService { get; }

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

        protected static ConfigurationSetApiDataModel BuildValidConfigurationSetApiDataModel()
        {
            var model = new ConfigurationSetApiDataModel
            {
                Title = "an-article",
                Url = new Uri("https://localhost"),
                ContentItems = new List<IBaseContentItemModel>
                {
                    new ConfigurationItemApiDataModel
                    {
                        Title = "an-article",
                        Url = new Uri("https://localhost"),
                        Value = "a-value",
                    },
                },
            };

            return model;
        }

        protected ConfigurationSetModel BuildValidConfigurationSetModel()
        {
            var model = new ConfigurationSetModel()
            {
                Id = ContentIdForConfigurationSetUpdate,
                Etag = Guid.NewGuid().ToString(),
                Title = "an-article",
                Url = new Uri("https://localhost"),
            };

            return model;
        }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(Logger, FakeMapper, FakeCmsApiService, FakeConfigurationSetDocumentService);

            return service;
        }
    }
}
