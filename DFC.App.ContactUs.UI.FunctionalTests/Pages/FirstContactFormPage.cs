// <copyright file="FirstContactFormPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.Pages
{
    public class FirstContactFormPage : BasePage
    {
        private readonly PageInteractionHelper pageHelper;
        private readonly FormCompletionHelper formHelper;
        private readonly ScenarioContext context;
        private readonly ObjectContext objectContext;

        private readonly By queryPageTitle = By.ClassName("govuk-fieldset__heading");
        private readonly By message = By.Id("Message");
        private readonly By feedback = By.Id("Feedback");
        private readonly By continueButton = By.Id("send-feedback-details");
        private readonly By categoryErrorMessage = By.LinkText("Choose a category");
        private readonly By issueErrorMessage = By.Id("Message-error");

        public FirstContactFormPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us first contact form page cannot be initialised.");
            }

            this.pageHelper = this.context.Get<PageInteractionHelper>();
            this.formHelper = this.context.Get<FormCompletionHelper>();
            this.objectContext = this.context.Get<ObjectContext>();
        }

        protected override string PageTitle => string.Empty;

        private List<IWebElement> OptionsList => this.pageHelper.FindElements(By.ClassName("govuk-radios__input"));

        public void VerifyQueryPage()
        {
            if (this.objectContext.Get("SelectOption") == "Contact an adviser")
            {
                this.pageHelper.VerifyText(this.queryPageTitle, "What is your query about?").Should().BeTrue();
            }
            else if (this.objectContext.Get("SelectOption") == "Give feedback")
            {
                this.pageHelper.VerifyText(this.queryPageTitle, "What is your feedback about?").Should().BeTrue();
            }
        }

        public FirstContactFormPage SelectQueryOption(string strOption)
        {
            {
                if (!string.IsNullOrWhiteSpace(strOption))
                {
                    string optionText = strOption.Replace(" ", string.Empty, StringComparison.CurrentCulture).ToUpper(CultureInfo.CurrentCulture);
                    foreach (var button in this.OptionsList)
                    {
                        var buttonText = button.GetAttribute("value").Replace(" ", string.Empty, StringComparison.CurrentCulture).ToUpper(CultureInfo.CurrentCulture);
                        if (buttonText.Contains(optionText, StringComparison.CurrentCulture))
                        {
                            button.Click();
                        }
                    }
                }

                return this;
            }
        }

        public FirstContactFormPage EnterQuery(string strQuery)
        {
            string option = this.objectContext.Get("SelectOption");
            if (option.Equals("Contact an adviser", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.EnterText(this.message, strQuery);
            }
            else if (option.Equals("Give feedback", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.EnterText(this.feedback, strQuery);
            }

            return this;
        }

        public EnterDetailsPage ClickContinueFirstForm()
        {
            this.formHelper.ClickElement(this.continueButton);
            return new EnterDetailsPage(this.context);
        }

        public void VerifyErrorMessages()
        {
            this.pageHelper.VerifyText(this.categoryErrorMessage, "Choose a category").Should().BeTrue();
            this.pageHelper.VerifyText(this.issueErrorMessage, "Enter a message describing the issue").Should().BeTrue();
        }

        public FirstContactFormPage ClickContinueonError()
        {
            this.formHelper.ClickElement(this.continueButton);
            return this;
        }
    }
}