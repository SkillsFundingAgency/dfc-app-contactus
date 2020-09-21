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
        private readonly PageInteractionHelper pageHelper;
        private readonly By confirmPageTitle = By.ClassName("govuk-heading-xl");

        public ConfirmationPage(ScenarioContext context)
            : base(context)
        {
            if (context != null)
            {
                this.pageHelper = context.Get<PageInteractionHelper>();
            }
            else
            {
                throw new NullReferenceException("The scenario context is null. The contact us confirmation page cannot be initialised.");
            }
        }

        protected override string PageTitle => string.Empty;

        public void VerifyConfirmPage()
        {
            this.pageHelper.VerifyText(this.confirmPageTitle, "Thank you for contacting us").Should().BeTrue();
        }
    }
}
