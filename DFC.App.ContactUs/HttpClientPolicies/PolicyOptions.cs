<<<<<<< HEAD
﻿namespace DFC.App.ContactUs.HttpClientPolicies
{
=======
﻿using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.HttpClientPolicies
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class PolicyOptions
    {
        public CircuitBreakerPolicyOptions HttpCircuitBreaker { get; set; } = new CircuitBreakerPolicyOptions();

        public RetryPolicyOptions HttpRetry { get; set; } = new RetryPolicyOptions();
    }
}
