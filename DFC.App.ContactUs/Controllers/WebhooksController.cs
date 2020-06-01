using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.Services.CmsApiProcessorService.Contracts;
using DFC.App.ContactUs.Services.EventProcessorService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    [Route("api/webhook")]
    public class WebhooksController : Controller
    {
        private readonly Dictionary<string, CacheOperation> acceptedEventTypes = new Dictionary<string, CacheOperation>
        {
            { "draft", CacheOperation.CreateOrUpdate },
            { "published", CacheOperation.CreateOrUpdate },
            { "draft-discarded", CacheOperation.Delete },
            { "unpublished", CacheOperation.Delete },
            { "deleted", CacheOperation.Delete },
        };

        private readonly ILogger<WebhooksController> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService<ContentPageModel> eventMessageService;
        private readonly ICmsApiService cmsApiService;

        public WebhooksController(ILogger<WebhooksController> logger, AutoMapper.IMapper mapper, IEventMessageService<ContentPageModel> eventMessageService, ICmsApiService cmsApiService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.eventMessageService = eventMessageService;
            this.cmsApiService = cmsApiService;
        }

        private enum CacheOperation
        {
            None,
            CreateOrUpdate,
            Delete,
        }

        [HttpPost]
        [Route("ReceiveContactUsEvents")]
        public async Task<IActionResult> ReceiveContactUsEvents()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string requestContent = await reader.ReadToEndAsync().ConfigureAwait(false);
            logger.LogInformation($"Received events: {requestContent}");

            var eventGridSubscriber = new EventGridSubscriber();
            foreach (var key in acceptedEventTypes.Keys)
            {
                eventGridSubscriber.AddOrUpdateCustomEventMapping(key, typeof(EventGridEventData));
            }

            var eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (var eventGridEvent in eventGridEvents)
            {
                if (!Guid.TryParse(eventGridEvent.Id, out Guid eventId))
                {
                    throw new InvalidDataException($"Invalid Guid for EventGridEvent.Id '{eventGridEvent.Id}'");
                }

                if (eventGridEvent.Data is SubscriptionValidationEventData subscriptionValidationEventData)
                {
                    logger.LogInformation($"Got SubscriptionValidation event data, validationCode: {subscriptionValidationEventData!.ValidationCode},  validationUrl: {subscriptionValidationEventData.ValidationUrl}, topic: {eventGridEvent.Topic}");

                    // Do any additional validation (as required) such as validating that the Azure resource ID of the topic matches
                    // the expected topic and then return back the below response
                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = subscriptionValidationEventData.ValidationCode,
                    };

                    return Ok(responseData);
                }
                else if (eventGridEvent.Data is EventGridEventData eventGridEventData)
                {
                    if (!Guid.TryParse(eventGridEventData.ItemId, out Guid contentPageId))
                    {
                        throw new InvalidDataException($"Invalid Guid for EventGridEvent.Data.ItemId '{eventGridEventData.ItemId}'");
                    }

                    if (!Uri.TryCreate(eventGridEventData.Api, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{eventGridEventData.Api}' received for Event Id: {eventId}");
                    }

                    var cacheOperation = acceptedEventTypes[eventGridEvent.EventType];

                    logger.LogInformation($"Got Event Id: {eventId}: {eventGridEvent.EventType}: Cache operation: {cacheOperation} {url}");

                    var result = await ProcessMessageAsync(cacheOperation, eventId, contentPageId, url).ConfigureAwait(false);

                    LogResult(eventId, contentPageId, result);
                }
                else
                {
                    throw new InvalidDataException($"Invalid event type '{eventGridEvent.EventType}' received for Event Id: {eventId}, should be one of '{string.Join(",", acceptedEventTypes.Keys)}'");
                }
            }

            return Ok();
        }

        private async Task<HttpStatusCode> ProcessMessageAsync(CacheOperation cacheOperation, Guid eventId, Guid contentPageId, Uri url)
        {
            switch (cacheOperation)
            {
                case CacheOperation.Delete:
                    return await eventMessageService.DeleteAsync(contentPageId).ConfigureAwait(false);
                case CacheOperation.CreateOrUpdate:
                    var apiDataModel = await cmsApiService.GetItemAsync(url).ConfigureAwait(false);
                    var contentPageModel = mapper.Map<ContentPageModel>(apiDataModel);

                    if (contentPageModel == null)
                    {
                        return HttpStatusCode.NoContent;
                    }

                    if (!TryValidateModel(contentPageModel))
                    {
                        return HttpStatusCode.BadRequest;
                    }

                    var result = await eventMessageService.UpdateAsync(contentPageModel).ConfigureAwait(false);

                    if (result == HttpStatusCode.NotFound)
                    {
                        result = await eventMessageService.CreateAsync(contentPageModel).ConfigureAwait(false);
                    }

                    return result;

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {cacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        private void LogResult(Guid eventId, Guid contentPageId, HttpStatusCode result)
        {
            switch (result)
            {
                case HttpStatusCode.OK:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Updated Content Page");
                    break;

                case HttpStatusCode.Created:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Created Content Page");
                    break;

                case HttpStatusCode.AlreadyReported:
                    logger.LogInformation($"Event Id: {eventId}, Content Page Id: {contentPageId}: Content Page previously updated");
                    break;

                default:
                    logger.LogWarning($"Event Id: {eventId}, Content Page Id: {contentPageId}: Content Page not Posted: Status: {result}");
                    break;
            }
        }
    }
}