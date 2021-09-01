using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.IntegrationTests.Fakes
{
    public class FakeWebhooksService : IWebhooksService
    {
        public Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            return Task.FromResult(HttpStatusCode.OK);
        }
    }
}
