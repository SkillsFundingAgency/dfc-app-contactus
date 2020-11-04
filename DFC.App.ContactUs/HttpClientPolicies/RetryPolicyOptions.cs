<<<<<<< HEAD
﻿namespace DFC.App.ContactUs.HttpClientPolicies
{
=======
﻿using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.HttpClientPolicies
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class RetryPolicyOptions
    {
        public int Count { get; set; } = 3;

        public int BackoffPower { get; set; } = 2;
    }
}
