<<<<<<< HEAD
﻿namespace DFC.App.ContactUs.Data.Models
{
=======
﻿using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models
{
    [ExcludeFromCodeCoverage]
>>>>>>> story/DFCC-1169-refresh-nugets
    public class SendGridSettings
    {
        public string? ApiKey { get; set; } = "unknown";

        public int DefaultNumberOfRetries { get; set; } = 2;

        public int DefaultMinimumBackOff { get; set; } = 2;

        public int DeltaBackOff { get; set; } = 3;

        public int DefaultMaximumBackOff { get; set; } = 3;
    }
}
