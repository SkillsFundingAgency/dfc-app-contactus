// <copyright file="ContactUsConfigurationSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium.Remote;
using System;
using System.Globalization;
using System.IO;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class ContactUsConfigurationSetup
    {
        private readonly ScenarioContext context;
        private readonly BrowserHelper browserHelper;

        public ContactUsConfigurationSetup(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration set up cannot be initialised.");
            }

            this.Configuration = new Configurator<AppSettings>();
            this.browserHelper = new BrowserHelper(this.Configuration.Data.BrowserConfiguration.BrowserName);
        }

        private Configurator<AppSettings> Configuration { get; set; }

        [BeforeScenario(Order = 0)]
        public void SetObjectContext(ObjectContext objectContext)
        {
            this.context.SetObjectContext(objectContext);
        }

        [BeforeScenario(Order = 1)]
        public void SetUpConfiguration()
        {
            this.context.SetConfiguration(this.Configuration);
        }

        [BeforeScenario(Order = 2)]
        public void SetupWebDriver()
        {
            var webDriver = new WebDriverConfigurator<AppSettings>(this.context).Create();
            webDriver.Manage().Window.Maximize();
            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(this.Configuration.Data.TimeoutConfiguration.PageNavigation);
            var currentWindow = webDriver.CurrentWindowHandle;
            webDriver.SwitchTo().Window(currentWindow);
            webDriver.Manage().Cookies.DeleteAllCookies();

            if (!this.browserHelper.IsExecutingInTheCloud())
            {
                var remoteWebDriver = webDriver as RemoteWebDriver;
                var capabilities = remoteWebDriver.Capabilities;
                var overriddenBrowserName = capabilities["browserName"] as string;
                var overriddenBrowserVersion = capabilities["browserVersion"] as string;
                this.Configuration.Data.BrowserConfiguration.BrowserName = overriddenBrowserName;
                this.Configuration.Data.BrowserConfiguration.BrowserVersion = overriddenBrowserVersion;
            }

            this.context.SetWebDriver(webDriver);
        }

        [BeforeScenario(Order = 3)]
        public void SetUpHelpers()
        {
            var webDriver = this.context.GetWebDriver();
            var webDriverwaitHelper = new WebDriverWaitHelper(webDriver, this.Configuration.Data.TimeoutConfiguration);
            var retryHelper = new RetryHelper(webDriver);
            this.context.Set(new SqlDatabaseConnectionHelper());
            this.context.Set(new PageInteractionHelper(webDriver, webDriverwaitHelper, retryHelper));
            this.context.Set(new AxeHelper(webDriver));
            var formCompletionHelper = new FormCompletionHelper(webDriver, webDriverwaitHelper, retryHelper);
            this.context.Set(formCompletionHelper);
            this.context.Set(new TableRowHelper(webDriver, formCompletionHelper));
            this.context.Set(new JavaScriptHelper(webDriver));
            this.context.Set(new RandomDataGenerator());
            this.context.Set(new RegexHelper());
            this.context.Set(new AssertHelper());
            this.context.Set(new ScreenShotTitleGenerator(0));
        }

        [BeforeScenario(Order = 4)]
        public void SetUpScreenshotDirectory()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory
             + "../../"
             + "Project\\Screenshots\\"
             + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture)
             + "\\";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            this.context.GetObjectContext().SetDirectory(directory);
        }
    }
}
