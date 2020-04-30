using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard.Attributes;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.MessageFunctionApp.Functions
{
    public static class MessageHandler
    {
        private static readonly string ClassFullName = typeof(MessageHandler).FullName!;

        [FunctionName("MessageHandler")]
        public static async Task Run(
            [ServiceBusTrigger("%cms-messages-topic%", "%cms-messages-subscription%", Connection = "service-bus-connection-string")] Message? message,
            [Inject] IMessageProcessor? messageProcessor,
            [Inject] IMessagePropertiesService? messagePropertiesService,
            ILogger log)
        {
            const string ActionTypeName = "ActionType";
            const string ContentTypeName = "CType";

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (messageProcessor == null)
            {
                throw new ArgumentNullException(nameof(messageProcessor));
            }

            if (messagePropertiesService == null)
            {
                throw new ArgumentNullException(nameof(messagePropertiesService));
            }

            message.UserProperties.TryGetValue(ActionTypeName, out var actionType);
            message.UserProperties.TryGetValue(ContentTypeName, out var contentType);
            message.UserProperties.TryGetValue("Id", out var messageContentId);

            var actionTypeString = actionType?.ToString() ?? string.Empty;
            var contentTypeString = contentType?.ToString() ?? string.Empty;

            // logger should allow setting up correlation id and should be picked up from message
            log.LogInformation($"{nameof(MessageHandler)}: Received message action '{actionTypeString}' for type '{contentType}' with Id: '{messageContentId}': Correlation id {message.CorrelationId}");

            var messageBody = Encoding.UTF8.GetString(message.Body);

            if (string.IsNullOrWhiteSpace(messageBody))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            if (!Enum.IsDefined(typeof(MessageAction), actionTypeString))
            {
                throw new ArgumentOutOfRangeException(nameof(message), $"Invalid message action type '{actionTypeString}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageAction)))}'");
            }

            if (!Enum.IsDefined(typeof(MessageContentType), contentTypeString))
            {
                throw new ArgumentOutOfRangeException(nameof(message), $"Invalid message content type '{contentTypeString}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageContentType)))}'");
            }

            var messageAction = Enum.Parse<MessageAction>(actionTypeString, true);
            var messageContentType = Enum.Parse<MessageContentType>(contentTypeString, true);
            var sequenceNumber = messagePropertiesService.GetSequenceNumber(message!);

            var result = await messageProcessor.ProcessAsync(messageBody, sequenceNumber, messageContentType, messageAction).ConfigureAwait(false);

            switch (result)
            {
                case HttpStatusCode.OK:
                    log.LogInformation($"{ClassFullName}: Content Page Id: {messageContentId}: Updated Content Page");
                    break;

                case HttpStatusCode.Created:
                    log.LogInformation($"{ClassFullName}: Content Page Id: {messageContentId}: Created Content Page");
                    break;

                case HttpStatusCode.AlreadyReported:
                    log.LogInformation($"{ClassFullName}: Content Page Id: {messageContentId}: Content Page previously updated");
                    break;

                default:
                    log.LogWarning($"{ClassFullName}: Content Page Id: {messageContentId}: Content Page not Posted: Status: {result}");
                    break;
            }
        }
    }
}
