using DFC.App.ContactUs.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.MessageFunctionApp.Services
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly IHttpClientService httpClientService;
        private readonly IMappingService mappingService;

        public MessageProcessor(IHttpClientService httpClientService, IMappingService mappingService)
        {
            this.httpClientService = httpClientService;
            this.mappingService = mappingService;
        }

        public async Task<HttpStatusCode> ProcessAsync(string message, long sequenceNumber, MessageContentType messageContentType, MessageAction messageAction)
        {
            switch (messageContentType)
            {
                case MessageContentType.Pages:
                    return await ProcessItemAsync(message, messageAction, sequenceNumber).ConfigureAwait(false);

                default:
                    throw new ArgumentOutOfRangeException(nameof(messageContentType), $"Unexpected content type '{messageContentType}'");
            }
        }

        private async Task<HttpStatusCode> ProcessItemAsync(string message, MessageAction messageAction, long sequenceNumber)
        {
            var contentPageModel = mappingService.MapToContentPageModel(message, sequenceNumber);

            switch (messageAction)
            {
                case MessageAction.Draft:
                case MessageAction.Published:
                    var result = await httpClientService.PutAsync(contentPageModel).ConfigureAwait(false);
                    if (result == HttpStatusCode.NotFound)
                    {
                        result = await httpClientService.PostAsync(contentPageModel).ConfigureAwait(false);
                    }

                    return result;

                case MessageAction.Deleted:
                    return await httpClientService.DeleteAsync(contentPageModel.DocumentId).ConfigureAwait(false);

                default:
                    throw new ArgumentOutOfRangeException(nameof(messageAction), $"Invalid message action '{messageAction}' received, should be one of '{string.Join(",", Enum.GetNames(typeof(MessageAction)))}'");
            }
        }
    }
}
