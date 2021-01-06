﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using Notify.Exceptions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailService
{
    [ExcludeFromCodeCoverage]
    public class NotifyEmailServices<TEmailRequestModel> : INotifyEmailServices<TEmailRequestModel>
      where TEmailRequestModel : class, IEmailRequestModel
    {
        private readonly ILogger<NotifyEmailServices<TEmailRequestModel>> logger;
        private readonly INotifyClientProxy notifyClientProxy;
        private readonly NotifyOptions notifyOptions;

        public NotifyEmailServices(
            ILogger<NotifyEmailServices<TEmailRequestModel>> logger,
            INotifyClientProxy notifyClientProxy, 
            NotifyOptions notifyOptions)
        {
            this.logger = logger;
            this.notifyClientProxy = notifyClientProxy;
            this.notifyOptions = notifyOptions;
        }

        public async Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel)
        {
            _ = emailRequestModel ?? throw new ArgumentNullException(nameof(emailRequestModel));

            emailRequestModel.ToEmailAddress = "gr2015@as-tech.co.uk";

            logger.LogInformation($"{nameof(SendEmailAsync)} sending email to {emailRequestModel.ToEmailAddress}");

            try
            {
                var response = notifyClientProxy.GetNotificationClient().SendEmail(emailRequestModel.ToEmailAddress, emailRequestModel.IsCallBack ? notifyOptions.CallMeBackTemplateId : notifyOptions.ByEmailTemplateId, emailRequestModel.PersonalisationMappings);
            }
            catch (NotifyClientException notifyClientException)
            {
                logger.LogError($"{nameof(SendEmailAsync)} had error sending to {emailRequestModel.ToEmailAddress} - {notifyClientException.Message} ");
                return false;
            }

            logger.LogInformation($"{nameof(SendEmailAsync)} sent email to {emailRequestModel.ToEmailAddress}");
            await Task.CompletedTask.ConfigureAwait(false);

            return true;
        }
    }
}
