﻿// <copyright file="FormSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.Helper;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    internal class FormSteps
    {
        public FormSteps(ScenarioContext context)
        {
            this.Context = context;
        }

        private ScenarioContext Context { get; set; }

        [When(@"I select the radio button option (.*)")]
        public void WhenISelectTheRadioButtonOption(string radioButtonLabel)
        {
            if (!this.InteractWithRadioButtonOrCheckbox(radioButtonLabel))
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label could not be found.");
            }
        }

        [When(@"I click the checkbox option (.*)")]
        public void WhenIClickTheCheckboxOption(string checkboxLabel)
        {
            if (!this.InteractWithRadioButtonOrCheckbox(checkboxLabel))
            {
                throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label could not be found.");
            }
        }

        [When(@"I enter (.*) in the (.*) field")]
        public void WhenIEnterInTheField(string text, string fieldLabel)
        {
            var allLabels = this.Context.GetWebDriver().FindElements(By.TagName("label"));
            foreach (var label in allLabels)
            {
                var labelText = label.Text;
                labelText = labelText.Replace("\r\n", " ", System.StringComparison.CurrentCultureIgnoreCase);
                labelText = labelText.Replace("\r\n", " ", System.StringComparison.CurrentCultureIgnoreCase);
                if (labelText.Trim().Equals(fieldLabel, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    var originalTimeout = this.Context.GetWebDriver().Manage().Timeouts().ImplicitWait;

                    var parentNode = this.Context.GetHelperLibrary<AppSettings>().JavaScriptHelper.GetParentElement(label);
                    this.Context.GetWebDriver().Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                    var fields = parentNode.FindElements(By.TagName("input"));
                    this.Context.GetWebDriver().Manage().Timeouts().ImplicitWait = originalTimeout;

                    if (fields.Count > 0)
                    {
                        fields[0].SendKeys(text);
                        return;
                    }
                    else
                    {
                        var textAreas = parentNode.FindElements(By.TagName("textarea"));

                        if (textAreas.Count > 0)
                        {
                            textAreas[0].SendKeys(text);
                        }
                        else
                        {
                            throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label was found but an editable text field could not be found.");
                        }
                    }

                    return;
                }
            }

            throw new NotFoundException($"Unable to perform the step: {this.Context.StepContext.StepInfo.Text}. The label could not be found.");
        }

        /// <summary>
        /// Click the checkbox or the radio button based on the inputs label.
        /// </summary>
        /// <param name="inputLabelText">Label text for the radio button or the checkbox (whichever applies).</param>
        /// <returns>Success boolean.</returns>
        private bool InteractWithRadioButtonOrCheckbox(string inputLabelText)
        {
            var allLabels = this.Context.GetWebDriver().FindElements(By.TagName("label"));
            foreach (var label in allLabels)
            {
                if (label.Text.Trim().Equals(inputLabelText, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    var parentNode = this.Context.GetHelperLibrary<AppSettings>().JavaScriptHelper.GetParentElement(label);
                    var input = parentNode.FindElement(By.TagName("input"));
                    input.Click();
                    return true;
                }
            }

            return false;
        }
    }
}
