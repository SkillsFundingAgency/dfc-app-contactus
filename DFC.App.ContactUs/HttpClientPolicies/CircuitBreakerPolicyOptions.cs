using System;
<<<<<<< HEAD

namespace DFC.App.ContactUs.HttpClientPolicies
{
=======
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.HttpClientPolicies
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class CircuitBreakerPolicyOptions
    {
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);

        public int ExceptionsAllowedBeforeBreaking { get; set; } = 12;
    }
}
