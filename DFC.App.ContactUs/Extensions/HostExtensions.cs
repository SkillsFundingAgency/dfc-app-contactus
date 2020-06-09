using DFC.App.ContactUs.TelemetryInitializer;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.App.ContactUs.Extensions
{
    public static class HostExtensions
    {
        public static IWebHost AddApplicationTelemetryInitializer(this IWebHost host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var telemetryConfig = (TelemetryConfiguration)host.Services.GetService(typeof(TelemetryConfiguration));
            var logger = (ILogger<ApplicationTelemetryInitializer>)host.Services.GetService(typeof(ILogger<ApplicationTelemetryInitializer>));
            var configuration = (IConfiguration)host.Services.GetService(typeof(IConfiguration));
            telemetryConfig.TelemetryInitializers.Add(new ApplicationTelemetryInitializer(logger, configuration));

            return host;
        }
    }
}
