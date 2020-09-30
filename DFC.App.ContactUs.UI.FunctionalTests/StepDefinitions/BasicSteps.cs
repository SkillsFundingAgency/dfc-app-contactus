// <copyright file="BasicSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
using System.Linq;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class BasicSteps
    {
        private ScenarioContext context;

        public BasicSteps(ScenarioContext context)
        {
            this.context = context;
        }

        [When(@"I click the (.*) button")]
        public void WhenIClickTheButton(string buttonText)
        {
            var allbuttons = this.context.GetWebDriver().FindElements(By.TagName("button")).ToList();

            foreach (var button in allbuttons)
            {
                if (button.Text.Trim().Equals(buttonText, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    button.Click();
                    return;
                }
            }

            throw new NotFoundException($"Unable to perform the step: {this.context.StepContext.StepInfo.Text}. The button could not be found.");
        }
    }
}