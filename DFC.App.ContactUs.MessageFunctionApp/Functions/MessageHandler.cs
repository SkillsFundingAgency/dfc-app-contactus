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
        private static readonly string ClassFullName = typeof(MessageHandler).FullName;

        [FunctionName("MessageHandler")]
        public static async Task Run(
            [ServiceBusTrigger("%cms-messages-topic%", "%cms-messages-subscription%", Connection = "service-bus-connection-string")] Message message,
            [Inject] IMessageProcessor messageProcessor,
            [Inject] IMessagePropertiesService messagePropertiesService,
            ILogger log)
        {
            const string ActionTypeName = "ActionType";
            const string ContentTypeName = "CType";

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            message.UserProperties.TryGetValue(ActionTypeName, out var actionType);
            message.UserProperties.TryGetValue(ContentTypeName, out var contentType);
            message.UserProperties.TryGetValue("Id", out var messageContentId);

            // logger should allow setting up correlation id and should be picked up from message
            log.LogInformation($"{nameof(MessageHandler)}: Received message action '{actionType}' for type '{contentType}' with Id: '{messageContentId}': Correlation id {message.CorrelationId}");

            var messageBody = Encoding.UTF8.GetString(message?.Body);

            if (string.IsNullOrWhiteSpace(messageBody))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            if (!Enum.IsDefined(typeof(MessageAction), actionType?.ToString()))
            {
                throw new ArgumentOutOfRangeException(ActionTypeName, $"Invalid message action '{actionType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageAction)))}'");
            }

            if (!Enum.IsDefined(typeof(MessageContentType), contentType?.ToString()))
            {
                throw new ArgumentOutOfRangeException(ContentTypeName, $"Invalid message content type '{contentType}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageContentType)))}'");
            }

            var messageAction = Enum.Parse<MessageAction>(actionType?.ToString());
            var messageContentType = Enum.Parse<MessageContentType>(contentType?.ToString());
            var sequenceNumber = messagePropertiesService.GetSequenceNumber(message);

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
