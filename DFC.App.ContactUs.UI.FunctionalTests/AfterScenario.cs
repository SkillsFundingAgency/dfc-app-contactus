// <copyright file="TearDown.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class AfterScenario
    {
        public AfterScenario(ScenarioContext context)
        {
            this.Context = context;

            if (this.Context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration TearDown class cannot be initialised.");
            }

            this.WebDriver = this.Context.GetWebDriver();
        }

        private ScenarioContext Context { get; set; }

        private IWebDriver WebDriver { get; set; }

        [AfterScenario(Order = 0)]
        public void TakeScreenshotOnFailure()
        {
            if (this.Context.TestError != null)
            {
                this.Context.GetHelperLibrary().ScreenshotHelper.TakeScreenshot();
            }
        }

        [AfterScenario(Order = 1)]
        public void InformBrowserStackOnFailure()
        {
            if (this.Context.TestError != null && this.Context.GetHelperLibrary().BrowserHelper.IsExecutingInTheCloud())
            {
                var browserStackReport = new BrowserStackReporter(this.Context.GetConfiguration<AppSettings>().Data.BrowserStackConfiguration, ((RemoteWebDriver)this.Context.GetWebDriver()).SessionId.ToString());
                browserStackReport.SendMessage("failed", this.Context.TestError.Message);
            }
        }

        [AfterScenario(Order = 2)]
        public void DisposeWebDriver()
        {
            if (!this.Context.GetHelperLibrary().BrowserHelper.IsExecutingInTheCloud())
            {
                this.WebDriver?.Quit();
                this.WebDriver?.Dispose();
            }
        }
    }
}
