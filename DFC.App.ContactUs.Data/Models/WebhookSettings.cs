using System;
using System.Collections.Generic;
<<<<<<< HEAD

namespace DFC.App.ContactUs.Data.Models
{
=======
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class WebhookSettings
    {
        public Uri? SubscriptionApiEndpointUrl { get; set; }

        public Uri? ApplicationWebhookReceiverEndpointUrl { get; set; }

        public List<string>? IncludeEventTypes { get; set; }
    }
}
