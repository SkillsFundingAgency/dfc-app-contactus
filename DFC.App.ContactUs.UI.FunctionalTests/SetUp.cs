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
    public class SetUp
    {
        private readonly ScenarioContext context;
        private readonly BrowserHelper browserHelper;

        public SetUp(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The SetUp class cannot be initialised.");
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
            var javaScriptHelper = new JavaScriptHelper(this.context.GetWebDriver());
            var webDriverWaitHelper = new WebDriverWaitHelper(this.context.GetWebDriver(), this.context.GetConfiguration<AppSettings>().Data.TimeoutConfiguration, javaScriptHelper);
            var retryHelper = new RetryHelper();
            var axeHelper = new AxeHelper(this.context.GetWebDriver());
            var browserHelper = new BrowserHelper(this.context.GetConfiguration<AppSettings>().Data.BrowserConfiguration.BrowserName);
            var formCompletionHelper = new FormCompletionHelper(this.context.GetWebDriver(), webDriverWaitHelper, retryHelper, javaScriptHelper);
            var httpClientRequestHelper = new HttpClientRequestHelper("NEED AN ACCESS TOKEN");
            var pageInteractionHelper = new PageInteractionHelper(this.context.GetWebDriver(), webDriverWaitHelper, retryHelper);
            var mongoDbConnectionHelper = new MongoDbConnectionHelper(this.context.GetConfiguration<AppSettings>().Data.MongoDatabaseConfiguration);
            var sqlDatabaseConnectionHelper = new SqlDatabaseConnectionHelper("NEED A CONN STRING");
            var screenshotHelper = new ScreenshotHelper(this.context);

            this.context.SetHelperLibrary(new HelperLibrary(
                javaScriptHelper,
                webDriverWaitHelper,
                retryHelper,
                axeHelper,
                browserHelper,
                formCompletionHelper,
                httpClientRequestHelper,
                pageInteractionHelper,
                mongoDbConnectionHelper,
                screenshotHelper,
                sqlDatabaseConnectionHelper));
        }
    }
}
