using System;
using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Models
{
    public class WebhookSettings
    {
        public Uri? SubscriptionApiEndpointUrl { get; set; }

        public Uri? ApplicationWebhookReceiverEndpointUrl { get; set; }

        public List<string>? IncludeEventTypes { get; set; }
    }
}
