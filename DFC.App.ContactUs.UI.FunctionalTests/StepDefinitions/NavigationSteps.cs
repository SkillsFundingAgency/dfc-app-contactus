// <copyright file="NavigationSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.UI.FunctionalTests.Pages;
using DFC.App.ContactUs.UI.FunctionalTests.Utilities;
using DFC.TestAutomation.UI.Config;
using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class NavigationSteps
    {
        private ScenarioContext context;
        private PageInteractionHelper pageInteraction;

        public NavigationSteps(ScenarioContext context)
        {
            this.context = context;
            this.WebDriver = this.context.GetWebDriver();
            var webDriverWaitHelper = new WebDriverWaitHelper(this.WebDriver, this.context.Get<FrameworkConfig>().TimeOutConfig);
            var retryHelper = new RetryHelper(this.WebDriver);
            this.pageInteraction = new PageInteractionHelper(this.WebDriver, webDriverWaitHelper, retryHelper);
        }

        private IWebDriver WebDriver { get; set; }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnThePage(string pageName)
        {
            switch (pageName)
            {
                case ConstantString.PageName.LandingPage:
                    var contactUsHomePage = new ContactUsLandingPage(this.context);
                    contactUsHomePage.NavigateToContactUsPage();
                    this.pageInteraction.VerifyText(By.CssSelector("h1.govuk-fieldset__heading"), "Contact us");
                    break;

                default:
                    throw new OperationCanceledException($"Unable to perform the step: {this.context.StepContext.StepInfo.Text}. The page name provided was not recognised.");
            }
        }
    }
}
