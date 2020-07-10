using System;

namespace DFC.App.ContactUs.Data.Models.Subscription
{
    public class SubscriptionRequest
    {
        public string? Name { get; set; }

        public Uri? Endpoint { get; set; }

        public SubscriptionFilter? Filter { get; set; }
    }
}
