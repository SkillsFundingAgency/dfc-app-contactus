// <copyright file="ValidationSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Extension;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class ValidationSteps
    {
        private ScenarioContext context;

        public ValidationSteps(ScenarioContext context)
        {
            this.context = context;
        }

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

            var actualText = this.context.GetHelperLibrary().CommonActionHelper.GetText(locator);
            Assert.AreEqual(pageName, actualText);
        }
    }
}