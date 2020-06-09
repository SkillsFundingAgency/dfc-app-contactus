using DFC.App.ContactUs.Controllers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Models;
using DFC.App.ContactUs.Services.EventProcessorService.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DFC.App.ContactUs.UnitTests.ControllerTests.WebhooksControllerTests
{
    public abstract class BaseWebhooksControllerTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected const string ContentTypeContactUs = "contact-us";

        protected BaseWebhooksControllerTests()
        {
            Logger = A.Fake<ILogger<WebhooksController>>();
            FakeEventMessageService = A.Fake<IEventMessageService<ContentPageModel>>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        protected ILogger<WebhooksController> Logger { get; }

        protected IEventMessageService<ContentPageModel> FakeEventMessageService { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected static ContactUsApiDataModel BuildValidContactUsApiDataModel()
        {
            var model = new ContactUsApiDataModel()
            {
                ItemId = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                Content = "<h1>A document</h1>",
                Published = DateTime.UtcNow,
            };

            return model;
        }

        protected static ContentPageModel BuildValidContentPageModel()
        {
            var model = new ContentPageModel()
            {
                Id = Guid.NewGuid(),
                CanonicalName = "an-article",
                BreadcrumbTitle = "An article",
                IncludeInSitemap = true,
                Version = Guid.NewGuid(),
                Url = new Uri("https://localhost"),
                Content = "<h1>A document</h1>",
                LastReviewed = DateTime.UtcNow,
            };

            return model;
        }

        protected static EventGridEvent[] BuildValidEventGridEvent<TModel>(string eventType, TModel data)
        {
            var models = new EventGridEvent[]
            {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = $"{ContentTypeContactUs}/a-canonical-name",
                    Data = data,
                    EventType = eventType,
                    EventTime = DateTime.Now,
                    DataVersion = "1.0",
                },
            };

            return models;
        }

        protected static Stream BuildStreamFromModel<TModel>(TModel model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            MemoryStream stream = new MemoryStream(byteArray);

            return stream;
        }

        protected WebhooksController BuildWebhooksController(string mediaTypeName)
        {
            var objectValidator = A.Fake<IObjectModelValidator>();
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new WebhooksController(Logger, FakeMapper, FakeEventMessageService, FakeCmsApiService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
                ObjectValidator = objectValidator,
            };

            A.CallTo(() => controller.ObjectValidator.Validate(A<ActionContext>.Ignored, A<ValidationStateDictionary>.Ignored, A<string>.Ignored, A<object>.Ignored));

            return controller;
        }
    }
}
