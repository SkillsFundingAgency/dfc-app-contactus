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
        private readonly INotificationClient notificationClient;
        private readonly HttpClientWrapper httpClientWrapper;

        public NotifyClientProxy(NotifyOptions notifyOptions, HttpClient httpClient)
        {
            httpClientWrapper = new HttpClientWrapper(httpClient);
            _ = notifyOptions ?? throw new ArgumentNullException(nameof(notifyOptions));
            notificationClient = new NotificationClient(httpClientWrapper, notifyOptions.ApiKey);
        }

        public EmailNotificationResponse SendEmail(string toEmail, string templateId, System.Collections.Generic.Dictionary<string, dynamic> personalisation)
        {
           return notificationClient.SendEmail(toEmail, templateId, personalisation);
        }
    }
}
