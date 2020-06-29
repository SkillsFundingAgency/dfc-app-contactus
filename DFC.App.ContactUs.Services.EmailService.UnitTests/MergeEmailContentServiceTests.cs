using DFC.App.ContactUs.Data.Enums;
using DFC.App.ContactUs.Extensions;
using DFC.App.ContactUs.Models;
using DFC.App.ContactUs.ViewModels;
using System;
using System.Globalization;
using Xunit;

namespace DFC.App.ContactUs.Services.EmailService.UnitTests
{
    public class MergeEmailContentServiceTests
    {
        [Fact]
        public void MergeEmailContentServiceReturnsSuccess()
        {
            // arrange
            var contactUsEmailRequestModel = new ContactUsEmailRequestModel
            {
                GivenName = "First name",
                FamilyName = "Last name",
                FromEmailAddress = "me@me.com",
                TelephoneNumber = "0123456789",
                DateOfBirth = DateTime.Now.AddYears(-14),
                Postcode = "CV1 1CV",
                Query = "I need more details please",
                CallbackDateTime = $"{EnterYourDetailsBodyViewModel.DateLabels[CallbackDateOption.TodayPlus1]}, {CallbackTimeOption.Band3.GetDescription()}",
            };
            var content = $"{nameof(contactUsEmailRequestModel.GivenName)} = {{{nameof(contactUsEmailRequestModel.GivenName)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.FamilyName)} = {{{nameof(contactUsEmailRequestModel.FamilyName)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.FromEmailAddress)} = {{{nameof(contactUsEmailRequestModel.FromEmailAddress)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.TelephoneNumber)} = {{{nameof(contactUsEmailRequestModel.TelephoneNumber)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.DateOfBirth)} = {{{nameof(contactUsEmailRequestModel.DateOfBirth)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.Postcode)} = {{{nameof(contactUsEmailRequestModel.Postcode)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.Query)} = {{{nameof(contactUsEmailRequestModel.Query)}}}" + Environment.NewLine +
                          $"{nameof(contactUsEmailRequestModel.CallbackDateTime)} = {{{nameof(contactUsEmailRequestModel.CallbackDateTime)}}}";
            var expectedResult = content
                                .Replace($"{{{nameof(contactUsEmailRequestModel.GivenName)}}}", contactUsEmailRequestModel.GivenName, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.FamilyName)}}}", contactUsEmailRequestModel.FamilyName, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.FromEmailAddress)}}}", contactUsEmailRequestModel.FromEmailAddress, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.TelephoneNumber)}}}", contactUsEmailRequestModel.TelephoneNumber, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.DateOfBirth)}}}", contactUsEmailRequestModel.DateOfBirth.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.Postcode)}}}", contactUsEmailRequestModel.Postcode, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.Query)}}}", contactUsEmailRequestModel.Query, System.StringComparison.OrdinalIgnoreCase)
                                .Replace($"{{{nameof(contactUsEmailRequestModel.CallbackDateTime)}}}", contactUsEmailRequestModel.CallbackDateTime, System.StringComparison.OrdinalIgnoreCase);

            var mergeEmailContentService = new MergeEmailContentService();

            //// act
            var result = mergeEmailContentService.MergeTemplateBodyWithContent(contactUsEmailRequestModel, content);

            //// assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MergeEmailContentServiceReturnsContentWhenRequestModelIsNull()
        {
            // arrange
            ContactUsEmailRequestModel? contactUsEmailRequestModel = null;
            const string content = "the original content";
            const string expectedResult = content;

            var mergeEmailContentService = new MergeEmailContentService();

            //// act
            var result = mergeEmailContentService.MergeTemplateBodyWithContent(contactUsEmailRequestModel, content);

            //// assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void MergeEmailContentServiceReturnsNullWhenContentIsNull()
        {
            // arrange
            var contactUsEmailRequestModel = new ContactUsEmailRequestModel();
            const string? content = null;
            const string? expectedResult = content;

            var mergeEmailContentService = new MergeEmailContentService();

            //// act
            var result = mergeEmailContentService.MergeTemplateBodyWithContent(contactUsEmailRequestModel, content);

            //// assert
            Assert.Equal(expectedResult, result);
        }
    }
}
