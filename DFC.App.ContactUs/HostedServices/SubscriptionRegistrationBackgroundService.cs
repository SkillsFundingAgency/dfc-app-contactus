using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Data.Models.Subscription;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public SubscriptionRegistrationBackgroundService(IConfiguration configuration, WebhookSettings webhookSettings, IHttpClientFactory httpClientFactory, ILogger<SubscriptionRegistrationBackgroundService> logger)
        {
            this.configuration = configuration;
            this.webhookSettings = webhookSettings;
            this.httpClient = httpClientFactory.CreateClient();
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.RegisterSubscription().ConfigureAwait(false);
        }

        private async Task RegisterSubscription()
        {
            logger.LogInformation("Subscription registration started");

            var subscribeName = !string.IsNullOrEmpty(configuration["Configuration:ApplicationName"]) ? configuration["Configuration:ApplicationName"] : throw new ArgumentException("Configuration:ApplicationName not present in IConfiguration");

            var webhookReceiverUrl = $"{webhookSettings.ApplicationWebhookReceiverEndpointUrl ?? throw new ArgumentException(nameof(webhookSettings.ApplicationWebhookReceiverEndpointUrl))}api/webhook/receiveevents";

            logger.LogInformation($"Registering subscription for endpoint: {webhookReceiverUrl}");
            var subscriptionRequest = new SubscriptionRequest { Endpoint = new Uri(webhookReceiverUrl), Name = subscribeName.Replace(".", "-", StringComparison.CurrentCultureIgnoreCase), Filter = new SubscriptionFilter { PropertyContainsFilters = new List<SubscriptionPropertyContainsFilter>() { new SubscriptionPropertyContainsFilter { Key = "subject", Values = new List<string> { "email" }.ToArray() } } } };

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
