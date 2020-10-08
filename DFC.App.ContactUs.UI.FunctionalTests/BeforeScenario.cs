﻿// <copyright file="BeforeScenario.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.Helper;
using DFC.TestAutomation.UI.Settings;
using DFC.TestAutomation.UI.Support;
using OpenQA.Selenium.Remote;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class BeforeScenario
    {
        public BeforeScenario(ScenarioContext context)
        {
            this.Context = context;

            if (this.Context == null)
            {
                throw new NullReferenceException($"The scenario context is null. The {this.GetType().Name} class cannot be initialised.");
            }
        }

        private ScenarioContext Context { get; set; }

        [BeforeScenario(Order = 0)]
        public void SetObjectContext(ObjectContext objectContext)
        {
            this.Context.SetObjectContext(objectContext);
        }

        [BeforeScenario(Order = 1)]
        public void SetSettingsLibrary()
        {
            this.Context.SetSettingsLibrary(new SettingsLibrary<AppSettings>());
        }

        [BeforeScenario(Order = 2)]
        public void SetupWebDriver()
        {
            var settingsLibrary = this.Context.GetSettingsLibrary<AppSettings>();
            var webDriver = new WebDriverSupport<AppSettings>(this.Context).Create();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(settingsLibrary.TestExecutionSettings.TimeoutSettings.PageNavigation);
            webDriver.SwitchTo().Window(webDriver.CurrentWindowHandle);

            if (new BrowserHelper(settingsLibrary.BrowserSettings.BrowserName).IsExecutingInBrowserStack())
            {
                this.Context.SetWebDriver(webDriver as RemoteWebDriver);
                var capabilities = (this.Context.GetWebDriver() as RemoteWebDriver).Capabilities;
                var overriddenBrowserName = capabilities["browserName"] as string;
                var overriddenBrowserVersion = capabilities["browserVersion"] as string;
                settingsLibrary.BrowserSettings.BrowserName = overriddenBrowserName;
                settingsLibrary.BrowserSettings.BrowserVersion = overriddenBrowserVersion;
            }

            this.Context.SetWebDriver(webDriver);
        }

        [BeforeScenario(Order = 3)]
        public void SetUpHelpers()
        {
            var helperLibrary = new HelperLibrary<AppSettings>(this.Context.GetWebDriver(), this.Context.GetSettingsLibrary<AppSettings>());
            this.Context.SetHelperLibrary(helperLibrary);
        }
    }
}