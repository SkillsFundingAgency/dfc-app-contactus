using DFC.App.ContactUs.Data.Models;
using DFC.App.ContactUs.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Exceptions;
using Notify.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace DFC.App.ContactUs.Services.EmailService.UnitTests
{
    public class NotifyEmailServiceTests
    {
        private readonly ILogger<NotifyEmailService<ContactUsEmailRequestModel>> fakeLogger = A.Fake<ILogger<NotifyEmailService<ContactUsEmailRequestModel>>>();
        private readonly INotifyClientProxy fakeNotifyClientProxy = A.Fake<INotifyClientProxy>();

        [Fact]
        public void SendEmailAsyncReturnsSuccess()
        {
            // arrange
            var contactUsEmailRequestModel = new ContactUsEmailRequestModel()
            {
                ToEmailAddress = "abc@def.com",
            };

            var dummyNotifyOptions = new NotifyOptions();
            var notifyEmailService = new NotifyEmailService<ContactUsEmailRequestModel>(fakeLogger, fakeNotifyClientProxy, dummyNotifyOptions);
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Returns(A.Dummy<EmailNotificationResponse>());

            // act
            var result = notifyEmailService.SendEmailAsync(contactUsEmailRequestModel).Result;

            // assert
            result.Should().BeTrue();
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>.That.IsEqualTo(contactUsEmailRequestModel.ToEmailAddress), A<string>._, A<Dictionary<string, dynamic>>._)).MustHaveHappened();
        }


        [Theory]
        [InlineData(false, "ByEmailTemplateId")]
        [InlineData(true, "CallMeBackTemplateId")]

        public void SendEmailAsyncWithCorrectTemplateId(bool isCallBack, string expectedTemplateId)
        {
            // arrange
            var contactUsEmailRequestModel = new ContactUsEmailRequestModel()
            {
                IsCallBack = isCallBack,
            };

            var notifyOptions = new NotifyOptions()
            {
                ByEmailTemplateId = "ByEmailTemplateId",
                CallMeBackTemplateId = "CallMeBackTemplateId",
            };

            var notifyEmailService = new NotifyEmailService<ContactUsEmailRequestModel>(fakeLogger, fakeNotifyClientProxy, notifyOptions);
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Returns(A.Dummy<EmailNotificationResponse>());

            // act
            var result = notifyEmailService.SendEmailAsync(contactUsEmailRequestModel).Result;

            // assert
            result.Should().BeTrue();
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>._, A<string>.That.IsEqualTo(expectedTemplateId), A<Dictionary<string, dynamic>>._)).MustHaveHappened();
        }

        [Fact]
        public void SendEmailAsyncReturnsFailedOnError()
        {
            // arrange
            var contactUsEmailRequestModel = new ContactUsEmailRequestModel();

            var dummyNotifyOptions = new NotifyOptions();
            var notifyEmailService = new NotifyEmailService<ContactUsEmailRequestModel>(fakeLogger, fakeNotifyClientProxy, dummyNotifyOptions);
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).Throws<NotifyClientException>();

            // act
            var result = notifyEmailService.SendEmailAsync(contactUsEmailRequestModel).Result;

            // assert
            result.Should().BeFalse();
            A.CallTo(() => fakeNotifyClientProxy.SendEmail(A<string>._, A<string>._, A<Dictionary<string, dynamic>>._)).MustHaveHappened();
        }


    }
}
