﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
    public class WebhookSettings
    {
        public Uri? SubscriptionApiEndpointUrl { get; set; }

        public Uri? ApplicationWebhookReceiverEndpointUrl { get; set; }

        public List<string>? IncludeEventTypes { get; set; }
    }
}
