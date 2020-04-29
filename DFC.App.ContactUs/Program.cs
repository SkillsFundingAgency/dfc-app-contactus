using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.ContactUs
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args);

            webHost.Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var webHost = WebHost.CreateDefaultBuilder(args)
                 .ConfigureLogging((webHostBuilderContext, loggingBuilder) =>
                 {
                     //This filter is for app insights only
                     loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>(string.Empty, LogLevel.Trace);
                 })
                .UseStartup<Startup>();

            return webHost;
        }
    }
}
