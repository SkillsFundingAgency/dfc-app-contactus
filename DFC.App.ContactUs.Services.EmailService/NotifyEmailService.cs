﻿using DFC.App.ContactUs.Data.Contracts;
using DFC.App.ContactUs.Data.Models;
using Microsoft.Extensions.Logging;
using Notify.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DFC.App.ContactUs.Services.EmailService
{
    [ExcludeFromCodeCoverage]
    public class NotifyEmailService<TEmailRequestModel> : INotifyEmailService<TEmailRequestModel>
      where TEmailRequestModel : class, IEmailRequestModel
    {
        private readonly ILogger<NotifyEmailService<TEmailRequestModel>> logger;
        private readonly INotifyClientProxy notifyClientProxy;
        private readonly NotifyOptions notifyOptions;

        public NotifyEmailService(
            ILogger<NotifyEmailService<TEmailRequestModel>> logger,
            INotifyClientProxy notifyClientProxy,
            NotifyOptions notifyOptions)
        {
            _ = logger ?? throw new ArgumentNullException(nameof(logger));
            _ = notifyClientProxy ?? throw new ArgumentNullException(nameof(notifyClientProxy));
            _ = notifyOptions ?? throw new ArgumentNullException(nameof(notifyOptions));

            this.logger = logger;
            this.notifyClientProxy = notifyClientProxy;
            this.notifyOptions = notifyOptions;
        }

        public async Task<bool> SendEmailAsync(TEmailRequestModel? emailRequestModel)
        {
            _ = emailRequestModel ?? throw new ArgumentNullException(nameof(emailRequestModel));
            logger.LogInformation($"{nameof(SendEmailAsync)} sending email to {emailRequestModel.ToEmailAddress}");

            try
            {
                notifyClientProxy.SendEmail(emailRequestModel.ToEmailAddress!, emailRequestModel.IsCallBack! ? notifyOptions.CallMeBackTemplateId! : notifyOptions.ByEmailTemplateId!, emailRequestModel.PersonalisationMappings);
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
