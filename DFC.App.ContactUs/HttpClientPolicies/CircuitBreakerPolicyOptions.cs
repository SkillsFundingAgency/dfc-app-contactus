﻿using System;

namespace DFC.App.ContactUs.HttpClientPolicies
{
    public class CircuitBreakerPolicyOptions
    {
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);

        public int ExceptionsAllowedBeforeBreaking { get; set; } = 12;
    }
}
