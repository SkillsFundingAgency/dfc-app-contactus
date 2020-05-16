using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.PageService.EventProcessorServices;
using DFC.App.ContactUs.PageService.EventProcessorServices.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Controllers
{
    [Route("api/webhook")]
    public class WebhooksController : Controller
    {
        private const string EventTypePublished = "Published";
        private const string EventTypeDraft = "Draft";
        private const string EventTypeDeleted = "Deleted";

        private readonly ILogger<WebhooksController> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IEventMessageService eventMessageService;
        private readonly IApiDataProcessorService apiDataProcessorService;

        public WebhooksController(ILogger<WebhooksController> logger, AutoMapper.IMapper mapper, IEventMessageService eventMessageService, IApiDataProcessorService apiDataProcessorService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.eventMessageService = eventMessageService;
            this.apiDataProcessorService = apiDataProcessorService;
        }

        [HttpPost]
        [Route("ReceiveContactUsEvents")]
        public async Task<IActionResult> ReceiveContactUsEvents()
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            string requestContent = await reader.ReadToEndAsync().ConfigureAwait(false);
            logger.LogInformation($"Received events: {requestContent}");

            var eventGridSubscriber = new EventGridSubscriber();
            eventGridSubscriber.AddOrUpdateCustomEventMapping(EventTypePublished, typeof(EventGridEventData));
            eventGridSubscriber.AddOrUpdateCustomEventMapping(EventTypeDraft, typeof(EventGridEventData));
            eventGridSubscriber.AddOrUpdateCustomEventMapping(EventTypeDeleted, typeof(EventGridEventData));
            var eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (var eventGridEvent in eventGridEvents)
            {
                if (!Guid.TryParse(eventGridEvent.Id, out Guid id))
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
                    if (!Enum.IsDefined(typeof(MessageAction), eventGridEvent.EventType))
                    {
                        throw new InvalidDataException($"Invalid event type '{eventGridEvent.EventType}' received for Event Id: {id}, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageAction)))}'");
                    }

                    if (!Uri.TryCreate(eventGridEventData.Api, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{eventGridEventData.Api}' received for Event Id: {id}");
                    }

                    var eventType = Enum.Parse<MessageAction>(eventGridEvent.EventType, true);

                    logger.LogInformation($"Got Event Id: {id}: {eventType} {url}");

                    var result = await ProcessMessageAsync(eventType, id, url).ConfigureAwait(false);

                    LogResult(id, result);
                }
                else
                {
                    logger.LogWarning($"Event Id: {eventGridEvent.Id} with event type: {eventGridEvent.EventType}, contains unrecognised event data object");
                }
            }

            return Ok();
        }

        private async Task<HttpStatusCode> ProcessMessageAsync(MessageAction eventType, Guid id, Uri url)
        {
            switch (eventType)
            {
                case MessageAction.Deleted:
                    return await eventMessageService.DeleteAsync(id).ConfigureAwait(false);
                case MessageAction.Published:
                case MessageAction.Draft:
                    var apiDataModel = await apiDataProcessorService.GetAsync<ContactUsApiDataModel>(url).ConfigureAwait(false);
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
                    logger.LogError($"Got unknown event type - {eventType}");
                    return HttpStatusCode.BadRequest;
            }
        }

        private void LogResult(Guid id, HttpStatusCode result)
        {
            switch (result)
            {
                case HttpStatusCode.OK:
                    logger.LogInformation($"Content Page Id: {id}: Updated Content Page");
                    break;

                case HttpStatusCode.Created:
                    logger.LogInformation($"Content Page Id: {id}: Created Content Page");
                    break;

                case HttpStatusCode.AlreadyReported:
                    logger.LogInformation($"Content Page Id: {id}: Content Page previously updated");
                    break;

                default:
                    logger.LogWarning($"Content Page Id: {id}: Content Page not Posted: Status: {result}");
                    break;
            }
        }
    }
}