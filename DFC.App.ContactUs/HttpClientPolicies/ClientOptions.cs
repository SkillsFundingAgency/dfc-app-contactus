using System;

namespace DFC.App.ContactUs.HttpClientPolicies
{
    public abstract class ClientOptions
    {
        public Uri? BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 10);         // default to 10 seconds

        public string? ApiKey { get; set; }
    }
}
