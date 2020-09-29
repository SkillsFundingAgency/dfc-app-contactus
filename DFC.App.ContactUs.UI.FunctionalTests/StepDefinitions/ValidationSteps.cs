// <copyright file="ValidationSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class ValidationSteps
    {
        private ScenarioContext context;
        private PageInteractionHelper pageInteraction;

        public ValidationSteps(ScenarioContext context)
        {
            this.context = context;
            this.WebDriver = this.context.GetWebDriver();
            var webDriverWaitHelper = new WebDriverWaitHelper(this.WebDriver, this.context.GetConfiguration().Data.TimeoutConfiguration);
            var retryHelper = new RetryHelper(this.WebDriver);
            this.pageInteraction = new PageInteractionHelper(this.WebDriver, webDriverWaitHelper, retryHelper);
        }

        private IWebDriver WebDriver { get; set; }

        [Then(@"I am on the (.*) page")]
        public void ThenIAmOnThePage(string pageName)
        {
            By locator = null;

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "thank you for contacting us":
                    locator = By.CssSelector("h1.govuk-panel__title");
                    break;

                default:
                    locator = By.CssSelector("h1.govuk-fieldset__heading");
                    break;
            }

            this.pageInteraction.VerifyText(locator, pageName);
        }
    }
}