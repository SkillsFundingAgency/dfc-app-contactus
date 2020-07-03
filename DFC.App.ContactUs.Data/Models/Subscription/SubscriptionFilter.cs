using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models.Subscription
{
    [ExcludeFromCodeCoverage]
    public class SubscriptionFilter
    {
        public List<SubscriptionPropertyContainsFilter>? PropertyContainsFilters { get; set; }
    }
}
