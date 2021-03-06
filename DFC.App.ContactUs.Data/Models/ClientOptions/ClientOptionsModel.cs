﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs.Data.Models.ClientOptions
{
    [ExcludeFromCodeCoverage]
    public abstract class ClientOptionsModel
    {
        public Uri? BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = new TimeSpan(0, 0, 10);         // default to 10 seconds

        public string? ApiKey { get; set; }
    }
}