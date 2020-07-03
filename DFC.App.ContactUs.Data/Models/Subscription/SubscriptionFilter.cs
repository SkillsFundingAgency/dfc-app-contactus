using System.Collections.Generic;

namespace DFC.App.ContactUs.Data.Models.Subscription
{
    public class SubscriptionFilter
    {
        public List<SubscriptionPropertyContainsFilter>? PropertyContainsFilters { get; set; }
    }
}
