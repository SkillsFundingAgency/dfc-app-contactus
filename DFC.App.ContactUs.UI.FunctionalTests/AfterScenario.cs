﻿// <copyright file="AfterScenario.cs" company="PlaceholderCompany">
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
            if (this.Context.TestError != null && this.Context.GetSettingsLibrary<AppSettings>().AppSettings.TakeScreenshots)
            {
                this.Context.GetHelperLibrary().ScreenshotHelper.TakeScreenshot();
            }
        }

        [AfterScenario(Order = 1)]
        public void InformBrowserStackOnFailure()
        {
            if (this.Context.TestError != null)
            {
                if (this.Context.GetHelperLibrary().BrowserHelper.IsExecutingInTheCloud() && this.Context.GetRemoteWebDriver() != null)
                {
                    var browserStackSupport = new BrowserStackSupport<AppSettings>(this.Context.GetSettingsLibrary<AppSettings>());
                    browserStackSupport.SendMessage(this.Context.GetRemoteWebDriver(), "failed", this.Context.TestError.Message);
                }
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
