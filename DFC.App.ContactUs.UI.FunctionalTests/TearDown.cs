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
    public class TearDown
    {
        private readonly ScenarioContext context;

        public TearDown(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration TearDown class cannot be initialised.");
            }

            this.WebDriver = this.context.GetWebDriver();
        }

        private IWebDriver WebDriver { get; set; }

        [AfterScenario(Order = 0)]
        public void TakeScreenshotOnFailure()
        {
            if (this.context.TestError != null)
            {
                this.context.GetHelperLibrary().ScreenshotHelper.TakeScreenShot();
            }
        }

        [AfterScenario(Order = 1)]
        public void InformBrowserStackOnFailure()
        {
            if (this.context.TestError != null && this.context.GetHelperLibrary().BrowserHelper.IsExecutingInTheCloud())
            {
                var browserStackReport = new BrowserStackReport(this.context.GetConfiguration<AppSettings>().Data.BrowserStackConfiguration, ((RemoteWebDriver)this.context.GetWebDriver()).SessionId.ToString());
                browserStackReport.MarkTestAsFailed(this.context.TestError.Message);
            }
        }

        [AfterScenario(Order = 2)]
        public void DisposeWebDriver()
        {
            if (!this.context.GetHelperLibrary().BrowserHelper.IsExecutingInTheCloud())
            {
                this.WebDriver?.Quit();
                this.WebDriver?.Dispose();
            }
        }
    }
}
