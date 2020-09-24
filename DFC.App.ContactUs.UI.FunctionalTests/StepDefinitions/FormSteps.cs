// <copyright file="FormSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class FormSteps
    {
        private ScenarioContext context;
        private JavaScriptHelper javaScriptHelper;

        public FormSteps(ScenarioContext context)
        {
            this.context = context;
            this.WebDriver = this.context.GetWebDriver();
            this.javaScriptHelper = new JavaScriptHelper(this.WebDriver);
        }

        private IWebDriver WebDriver { get; set; }

        [When(@"I select the radio button option (.*)")]
        public void WhenISelectTheRadioButtonOption(string radioButtonLabel)
        {
            var allLabels = this.WebDriver.FindElements(By.TagName("label"));
            foreach (var label in allLabels)
            {
                if (label.Text.Trim().Equals(radioButtonLabel, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    var parentNode = this.javaScriptHelper.ExecuteScript("return arguments[0].parentNode;", label) as IWebElement;
                    var radioButton = parentNode.FindElement(By.TagName("input"));
                    radioButton.Click();
                    return;
                }
            }

            throw new NotFoundException($"Unable to perform the step: {this.context.StepContext.StepInfo.Text}. The label could not be found.");
        }

    }
}
