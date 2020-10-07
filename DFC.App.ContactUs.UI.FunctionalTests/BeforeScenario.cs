// <copyright file="BeforeScenario.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
                throw new NullReferenceException("The scenario context is null. The SetUp class cannot be initialised.");
            }

            this.SettingsLibrary = new SettingsLibrary<AppSettings>();
        }

        private SettingsLibrary<AppSettings> SettingsLibrary { get; set; }

        private ScenarioContext Context { get; set; }

        [BeforeScenario(Order = 0)]
        public void SetObjectContext(ObjectContext objectContext)
        {
            this.Context.SetObjectContext(objectContext);
        }

        [BeforeScenario(Order = 1)]
        public void SetSettingsLibrary()
        {
            this.Context.SetSettingsLibrary(this.SettingsLibrary);
        }

        [BeforeScenario(Order = 2)]
        public void SetupWebDriver()
        {
            var webDriver = new WebDriverSupport<AppSettings>(this.Context).Create();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(this.SettingsLibrary.TestExecutionSettings.TimeoutSettings.PageNavigation);
            webDriver.SwitchTo().Window(webDriver.CurrentWindowHandle);

            if (new BrowserHelper(this.SettingsLibrary.BrowserSettings.BrowserName).IsExecutingInBrowserStack())
            {
                this.Context.SetWebDriver(webDriver as RemoteWebDriver);
                var capabilities = (this.Context.GetWebDriver() as RemoteWebDriver).Capabilities;
                var overriddenBrowserName = capabilities["browserName"] as string;
                var overriddenBrowserVersion = capabilities["browserVersion"] as string;
                this.SettingsLibrary.BrowserSettings.BrowserName = overriddenBrowserName;
                this.SettingsLibrary.BrowserSettings.BrowserVersion = overriddenBrowserVersion;
            }

            this.Context.SetWebDriver(webDriver);
        }

        [BeforeScenario(Order = 3)]
        public void SetUpHelpers()
        {
            this.Context.SetHelperLibrary(new HelperLibrary<AppSettings>(this.Context.GetWebDriver(), this.Context.GetSettingsLibrary<AppSettings>()));
        }
    }
}
