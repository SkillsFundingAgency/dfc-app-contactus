using DFC.App.ContactUs.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Data.Contracts
{
    public interface IWebhooksService
    {
        Task<HttpStatusCode> DeleteContentAsync(Guid contentId, string contentType);

        Task<HttpStatusCode> DeleteContentItemAsync(Guid contentItemId, string contentType);

        Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Uri url);
    }
}
