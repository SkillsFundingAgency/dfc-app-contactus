// <copyright file="ConfirmationPage.cs" company="PlaceholderCompany">
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
    public class ConfirmationPage : BasePage
    {
        private readonly ScenarioContext context;
        private readonly PageInteractionHelper pageHelper;
        private readonly By confirmPageTitle = By.ClassName("govuk-heading-xl");

        public ConfirmationPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us confirmation page cannot be initialised.");
            }

            this.pageHelper = this.context.Get<PageInteractionHelper>();
        }

        protected override string PageTitle => string.Empty;

        public void VerifyConfirmPage()
        {
            this.pageHelper.VerifyText(this.confirmPageTitle, "Thank you for contacting us").Should().BeTrue();
        }
    }
}
