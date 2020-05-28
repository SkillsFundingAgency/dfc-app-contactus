//using DFC.App.ContactUs.Models;
//using DFC.App.ContactUs.Services.EmailService.Contracts;
//using DFC.App.ContactUs.Services.Services.EmailService;
//using FakeItEasy;
//using Microsoft.Extensions.Logging;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System.Collections.Generic;
//using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.ContactUs.Services.EmailService.UnitTests
{
    public class SendGridEmailServiceTests
    {
        //private readonly ILogger<SendGridEmailService<ContactUsEmailRequestModel>> fakeLogger = A.Fake<ILogger<SendGridEmailService<ContactUsEmailRequestModel>>>();
        //private readonly IMergeEmailContentService fakeMergeEmailContentService = A.Fake<IMergeEmailContentService>();
        //private readonly ISendGridClient fakeSendGridClient = A.Fake<ISendGridClient>();

        [Fact(Skip = "Sprint 6 - DFCC 267 - Awaiting email development to prove up the code and these tests")]
        public async Task SendGridEmailServiceGetReturnsSuccess()
        {
            // arrange
            //var contactUsEmailRequestModel = A.Fake<ContactUsEmailRequestModel>();
            //var expectedSendEmailResponse = new Response(System.Net.HttpStatusCode.Accepted, null, null);

            //A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<IEmailRequestModel>.Ignored, A<string>.Ignored)).Returns(A.Fake<string>());
            //A.CallTo(() => MailHelper.CreateSingleEmailToMultipleRecipients(A<EmailAddress>.Ignored, A<List<EmailAddress>>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns(A.Fake<SendGridMessage>());
            //A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>.Ignored, A<CancellationToken>.Ignored)).Returns(expectedSendEmailResponse);

            //var sendGridEmailService = new SendGridEmailService<ContactUsEmailRequestModel>(fakeLogger, fakeMergeEmailContentService, fakeSendGridClient);

            ////// act
            //var result = await sendGridEmailService.SendEmailAsync(contactUsEmailRequestModel).ConfigureAwait(false);

            ////// assert
            //A.CallTo(() => fakeMergeEmailContentService.MergeTemplateBodyWithContent(A<IEmailRequestModel>.Ignored, A<string>.Ignored)).MustHaveHappenedTwiceExactly();
            //A.CallTo(() => MailHelper.CreateSingleEmailToMultipleRecipients(A<EmailAddress>.Ignored, A<List<EmailAddress>>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).MustHaveHappenedOnceExactly();
            //A.CallTo(() => fakeSendGridClient.SendEmailAsync(A<SendGridMessage>.Ignored, A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();
            //Assert.True(result);
        }
    }
}
