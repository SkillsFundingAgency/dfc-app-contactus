using DFC.App.ContactUs.Data.Helpers;
using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Data.Models.Subscription;
using DFC.Compui.Telemetry.HostedService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.HostedServices
{
    public class SubscriptionRegistrationBackgroundService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly WebhookSettings webhookSettings;
        private readonly HttpClient httpClient;
        private readonly ILogger<SubscriptionRegistrationBackgroundService> logger;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public SubscriptionRegistrationBackgroundService(IConfiguration configuration, WebhookSettings webhookSettings, IHttpClientFactory httpClientFactory, ILogger<SubscriptionRegistrationBackgroundService> logger, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.configuration = configuration;
            this.webhookSettings = webhookSettings;
            this.httpClient = httpClientFactory.CreateClient();
            this.logger = logger;
            this.hostedServiceTelemetryWrapper = hostedServiceTelemetryWrapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", webhookSettings.ApiKey);

            var subscriptionRegistrationTask = hostedServiceTelemetryWrapper.Execute(async () => await this.RegisterSubscription().ConfigureAwait(false), nameof(SubscriptionRegistrationBackgroundService));

            await subscriptionRegistrationTask.ConfigureAwait(false);

            if (!subscriptionRegistrationTask.IsCompletedSuccessfully)
            {
                logger.LogInformation("Subscription Registration didn't complete successfully");

                if (subscriptionRegistrationTask.Exception != null)
                {
                    logger.LogError(subscriptionRegistrationTask.Exception.ToString());
                    throw subscriptionRegistrationTask.Exception;
                }
            }
        }

        private async Task RegisterSubscription()
        {
            logger.LogInformation("Subscription registration started");

            var subscribeName = !string.IsNullOrEmpty(configuration["Configuration:ApplicationName"]) ? configuration["Configuration:ApplicationName"] : throw new ArgumentException("Configuration:ApplicationName not present in IConfiguration");

            var webhookReceiverUrl = $"{webhookSettings.ApplicationWebhookReceiverEndpointUrl ?? throw new ArgumentException(nameof(webhookSettings.ApplicationWebhookReceiverEndpointUrl))}";

            logger.LogInformation($"Registering subscription for endpoint: {webhookReceiverUrl}");

            var subscriptionRequest = new SubscriptionRequest { Endpoint = new Uri(webhookReceiverUrl), Name = subscribeName, Filter = new SubscriptionFilter { IncludeEventTypes = webhookSettings.IncludeEventTypes, PropertyContainsFilters = new List<SubscriptionPropertyContainsFilter>() { new SubscriptionPropertyContainsFilter { Key = "subject", Values = EmailKeyHelper.GetEmailKeys().Select(z => z.ToString()).ToArray() } } } };

            var content = new StringContent(JsonConvert.SerializeObject(subscriptionRequest), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync(webhookSettings.SubscriptionApiEndpointUrl, content).ConfigureAwait(false);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Add subscription returned unsuccessful status code: {result.StatusCode}");
            }

            content.Dispose();

            logger.LogInformation("Subscription registration completed");
        }
    }
}
