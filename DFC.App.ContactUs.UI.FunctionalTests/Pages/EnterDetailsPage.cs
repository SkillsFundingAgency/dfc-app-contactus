// <copyright file="EnterDetailsPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Globalization;
using TechTalk.SpecFlow;

namespace SFA.DFC.ContactUs.UITests.Project.Tests.Pages
{
    public class EnterDetailsPage : BasePage
    {
        private readonly PageInteractionHelper pageHelper;
        private readonly FormCompletionHelper formHelper;
        private readonly ScenarioContext context;
        private readonly ObjectContext objectContext;
        private readonly IWebDriver webDriver;

        private readonly By detailsPageTitle = By.ClassName("govuk-heading-xl");
        private readonly By firstName = By.Id("Firstname");
        private readonly By lastName = By.Id("Lastname");
        private readonly By emailAddress = By.Id("EmailAddress");
        private readonly By confirmEmail = By.Id("ConfirmEmailAddress");
        private readonly By day = By.Id("DateOfBirthDay");
        private readonly By month = By.Id("DateOfBirthMonth");
        private readonly By year = By.Id("DateOfBirthYear");
        private readonly By postCode = By.Id("Postcode");
        private readonly By contactYes = By.Id("radio-inline-1");
        private readonly By firstNameError = By.Id("Firstname-error");
        private readonly By lastNameError = By.Id("Lastname-error");
        private readonly By emailError = By.Id("EmailAddress-error");
        private readonly By postCodeError = By.Id("Postcode-error");
        private readonly By termsCondError = By.Id("AcceptTermsAndConditions-error");
        private readonly By birthDateError = By.ClassName("field-validation-error");
        private readonly By contactNo = By.Id("radio-inline-2");
        private readonly By sendButton = By.CssSelector("#userform .govuk-button");

        public EnterDetailsPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (context != null)
            {
                this.formHelper = this.context.Get<FormCompletionHelper>();
            }
            else
            {
                throw new NullReferenceException("The scenario context is null. The contact us enter details page cannot be initialised.");
            }

            this.webDriver = context.GetWebDriver();
            this.formHelper = context.Get<FormCompletionHelper>();
            this.objectContext = context.Get<ObjectContext>();
        }

        protected override string PageTitle => string.Empty;

        private IWebElement TermsCond => this.webDriver.FindElement(By.ClassName("govuk-checkboxes__input"));

        public EnterDetailsPage VerifyDetailsPage()
        {
            this.pageHelper.VerifyPage(this.detailsPageTitle, "Enter your details");
            return this;
        }

        public EnterDetailsPage ClickSendWithError()
        {
            this.formHelper.ClickElement(this.sendButton);
            return this;
        }

        public void CompleteForm(string fname, string email, string confemail, string dob, string postcode)
        {
            this.formHelper.EnterText(this.firstName, fname);
            this.formHelper.EnterText(this.lastName, this.GetEnvBuild());
            this.formHelper.EnterText(this.emailAddress, email);
            this.formHelper.EnterText(this.confirmEmail, confemail);
            DateTime birthday = DateTime.ParseExact(dob, "dd/M/yyyy", CultureInfo.InvariantCulture);
            this.formHelper.EnterText(this.day, birthday.Day.ToString(CultureInfo.CurrentCulture));
            this.formHelper.EnterText(this.month, birthday.Month.ToString(CultureInfo.CurrentCulture));
            this.formHelper.EnterText(this.year, birthday.Year.ToString(CultureInfo.CurrentCulture));
            this.formHelper.EnterText(this.postCode, postcode);
        }

        public string GetEnvBuild()
        {
            return this.objectContext.Get("EnvironmentName") + "-" + this.objectContext.Get("build");
        }

        public void CompleteFeedbackForm(string fname, string email, string confemail)
        {
            this.formHelper.EnterText(this.firstName, fname);
            this.formHelper.EnterText(this.lastName, this.GetEnvBuild());
            this.formHelper.EnterText(this.emailAddress, email);
            this.formHelper.EnterText(this.confirmEmail, confemail);
        }

        public void SelectTermsandConditions()
        {
            if (this.TermsCond.Selected == false)
             {
                 this.TermsCond.Click();
             }
        }

        public ConfirmationPage ClickSend()
        {
            this.formHelper.ClickElement(this.sendButton);
            return new ConfirmationPage(this.context);
        }

        public void SelectAddContact(string consent)
        {
            if (consent == null)
            {
                throw new NullReferenceException("The consent parameter must be set.");
            }

            if (consent.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.SelectRadioButton(this.contactYes);
            }
            else
            {
                this.formHelper.SelectRadioButton(this.contactNo);
            }
        }

        public void VerifyErrorMessages()
        {
            this.pageHelper.VerifyText(this.firstNameError, "Enter your first name").Should().BeTrue();
            this.pageHelper.VerifyText(this.lastNameError, "Enter your last name").Should().BeTrue();
            this.pageHelper.VerifyText(this.emailError, "Enter your email address").Should().BeTrue();
            this.pageHelper.VerifyText(this.postCodeError, "Enter your postcode").Should().BeTrue();
            this.pageHelper.VerifyText(this.termsCondError, "You must accept our Terms and Conditions").Should().BeTrue();
        }

        public void VerifyBirthDateErrorMessage()
        {
            this.pageHelper.VerifyText(this.birthDateError, "You must be over 13 to use this service").Should().BeTrue();
        }
    }
}
