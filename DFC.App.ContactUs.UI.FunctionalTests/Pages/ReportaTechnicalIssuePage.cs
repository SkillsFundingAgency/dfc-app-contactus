// <copyright file="ReportATechnicalIssuePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SFA.DFC.ContactUs.UITests.Project.Tests.Pages
{
    public class ReportATechnicalIssuePage : BasePage
    {
        private readonly PageInteractionHelper pageHelper;
        private readonly FormCompletionHelper formHelper;
        private readonly ScenarioContext context;

        private readonly By technicalPageTitle = By.ClassName("govuk-heading-xl");
        private readonly By message = By.Id("Message");
        private readonly By continueButton = By.CssSelector("#userform .govuk-button");

        public ReportATechnicalIssuePage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us report a technical issue page cannot be initialised.");
            }

            this.pageHelper = this.context.Get<PageInteractionHelper>();
            this.formHelper = this.context.Get<FormCompletionHelper>();
        }

        protected override string PageTitle => string.Empty;

        public void VerifyTechnicalPage()
        {
            this.pageHelper.VerifyText(this.technicalPageTitle, "Report a technical issue").Should().BeTrue();
        }

        public ReportATechnicalIssuePage EnterTechnicalQuery(string query)
        {
            this.formHelper.EnterText(this.message, query);
            return this;
        }

        public EnterDetailsPage ClickContinueonTechnicalForm()
        {
            this.formHelper.ClickElement(this.continueButton);
            return new EnterDetailsPage(this.context);
        }
    }
}
