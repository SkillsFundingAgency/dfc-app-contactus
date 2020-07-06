using System;

namespace DFC.App.ContactUs.Data.Models
{
    public class WebhookSettings
    {
        public Uri? SubscriptionsApiBaseAddress { get; set; }

        public Uri? WebhookReceiverEndpoint { get; set; }
    }
}
