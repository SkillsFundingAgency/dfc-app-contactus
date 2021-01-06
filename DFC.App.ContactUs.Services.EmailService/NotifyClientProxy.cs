using DFC.App.ContactUs.Data.Models;
using Notify.Client;
using Notify.Interfaces;
using System;
using System.Net.Http;

namespace DFC.App.ContactUs.Services.EmailService
{
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

        public INotificationClient GetNotificationClient()
        {
            return notificationClient;
        }
    }
}
