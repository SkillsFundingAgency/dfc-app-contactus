// <copyright file="SelectAnOptionPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.Pages
{
    public class SelectAnOptionPage : BasePage
    {
        private readonly FormCompletionHelper formHelper;
        private readonly ScenarioContext context;
        private readonly ObjectContext objectContext;

        private readonly By contactAdviser = By.Id("ContactOptionType_ContactAdviser");
        private readonly By technicalIssue = By.Id("ContactOptionType_Technical");
        private readonly By feedback = By.Id("ContactOptionType_Feedback");
        private readonly By continueButton = By.Id("show-basic-details");

        public SelectAnOptionPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us select an option page cannot be initialised.");
            }

            this.formHelper = this.context.Get<FormCompletionHelper>();
            this.objectContext = this.context.Get<ObjectContext>();
        }

        protected override string PageTitle => string.Empty;

        public SelectAnOptionPage SelectContactOption(string option)
        {
            if (option == null)
            {
                throw new NullReferenceException("The option parameter must be set.");
            }

            if (option.Equals("Contact an adviser", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.SelectRadioButton(this.contactAdviser);
            }
            else if (option.Equals("Report a technical issue", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.SelectRadioButton(this.technicalIssue);
            }
            else if (option.Equals("Give feedback", StringComparison.OrdinalIgnoreCase))
            {
                this.formHelper.SelectRadioButton(this.feedback);
            }

            this.objectContext.Set("SelectOption", option);
            return this;
        }

        public FirstContactFormPage ClickContinue()
        {
            this.formHelper.ClickElement(this.continueButton);
            return new FirstContactFormPage(this.context);
        }

        public ReportATechnicalIssuePage ClickContinueTechnical()
        {
            this.formHelper.ClickElement(this.continueButton);
            return new ReportATechnicalIssuePage(this.context);
        }
    }
}
