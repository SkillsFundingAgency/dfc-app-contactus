// <copyright file="NavigationSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.App.ContactUs.UI.FunctionalTests.Pages;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System;
using System.Globalization;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class NavigationSteps
    {
        private ScenarioContext context;

        public NavigationSteps(ScenarioContext context)
        {
            this.context = context;
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnThePage(string pageName)
        {
            switch (pageName.ToLower(CultureInfo.CurrentCulture))
            {
                case "contact us landing":
                    var contactUsHomePage = new ContactUsLandingPage(this.context);
                    contactUsHomePage.NavigateToContactUsPage();
                    var pageHeadingLocator = By.CssSelector("h1.govuk-fieldset__heading");
                    this.context.GetHelperLibrary<AppSettings>().WebDriverWaitHelper.WaitForElementToContainText(pageHeadingLocator, "Contact us");
                    break;

                default:
                    throw new OperationCanceledException($"Unable to perform the step: {this.context.StepContext.StepInfo.Text}. The page name provided was not recognised.");
            }
        }
    }
}
