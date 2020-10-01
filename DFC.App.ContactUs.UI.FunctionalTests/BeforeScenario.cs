// <copyright file="SetUp.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.Helper;
using DFC.TestAutomation.UI.TestSupport;
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

            this.Configuration = new Configurator<AppSettings>();
        }

        private Configurator<AppSettings> Configuration { get; set; }

        private ScenarioContext Context { get; set; }

        [BeforeScenario(Order = 0)]
        public void SetObjectContext(ObjectContext objectContext)
        {
            this.Context.SetObjectContext(objectContext);
        }

        [BeforeScenario(Order = 1)]
        public void SetUpConfiguration()
        {
            this.Context.SetConfiguration(this.Configuration);
        }

        [BeforeScenario(Order = 2)]
        public void SetupWebDriver()
        {
            var webDriver = new WebDriverConfigurator<AppSettings>(this.Context).Create();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(this.Configuration.Data.TimeoutConfiguration.PageNavigation);
            webDriver.Manage().Cookies.DeleteAllCookies();
            webDriver.SwitchTo().Window(webDriver.CurrentWindowHandle);

            if (new BrowserHelper(this.Configuration.Data.BrowserConfiguration.BrowserName).IsExecutingInTheCloud())
            {
                var remoteWebDriver = webDriver as RemoteWebDriver;
                var capabilities = remoteWebDriver.Capabilities;
                var overriddenBrowserName = capabilities["browserName"] as string;
                var overriddenBrowserVersion = capabilities["browserVersion"] as string;
                this.Configuration.Data.BrowserConfiguration.BrowserName = overriddenBrowserName;
                this.Configuration.Data.BrowserConfiguration.BrowserVersion = overriddenBrowserVersion;
            }

            this.Context.SetWebDriver(webDriver);
        }

        [BeforeScenario(Order = 3)]
        public void SetUpHelpers()
        {
            this.Context.SetHelperLibrary(new HelperLibraryConfigurator<AppSettings>(this.Context).CreateHelperLibrary());
        }
    }
}
