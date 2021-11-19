// <copyright file="ValidationSteps.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class ValidationSteps
    {
        public ValidationSteps(ScenarioContext context)
        {
            Context = context;
        }

        private ScenarioContext Context { get; set; }

        [Then(@"I am on the (.*) page")]
        public void ThenIAmOnThePage(string pageName)
        {
            By locator = null;

            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "thank you for contacting us":
                    locator = By.CssSelector("h1.govuk-panel__title");
                    break;

                case "send us a letter":
                    locator = By.CssSelector("h1");
                    break;

                default:
                    locator = By.CssSelector("h1.govuk-fieldset__heading");
                    break;
            }

            Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(locator, pageName);
        }

        [Then(@"I am taken to the webchat iFrame")]
        public void ThenIAmTakenToTheWebchatIFrame()
        {
            Context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToBeDisplayed(By.ClassName("dfc-app-contact-us-IframeContainer"));
        }
    }
}