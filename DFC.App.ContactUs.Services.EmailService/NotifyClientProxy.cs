using DFC.App.ContactUs.Data.Models;
using Notify.Client;
using Notify.Interfaces;
using Notify.Models.Responses;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace DFC.App.ContactUs.Services.EmailService
{
    [ExcludeFromCodeCoverage]
    public class NotifyClientProxy : INotifyClientProxy
    {
        private readonly HttpClient httpClient;
        private readonly NotifyOptions notifyOptions;

        public NotifyClientProxy(NotifyOptions notifyOptions, HttpClient httpClient)
        {
            _ = notifyOptions ?? throw new ArgumentNullException(nameof(notifyOptions));
            this.notifyOptions = notifyOptions;
            this.httpClient = httpClient;
        }

        public EmailNotificationResponse SendEmail(string toEmail, string templateId, System.Collections.Generic.Dictionary<string, dynamic> personalisation)
        {
            using (var httpClientWrapper = new HttpClientWrapper(httpClient))
            {
                var notificationClient = new NotificationClient(httpClientWrapper, notifyOptions.ApiKey);
                return notificationClient.SendEmail(toEmail, templateId, personalisation);
            }
        }
    }
}
