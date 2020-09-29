// <copyright file="ContactUsConfigurationSetup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Config;
using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class ContactUsConfigurationSetup
    {
        private readonly ScenarioContext context;

        public ContactUsConfigurationSetup(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration set up cannot be initialised.");
            }

            this.Configuration = new Configurator<AppSettings>();
        }

        private Configurator<AppSettings> Configuration { get; set; }

        private IWebDriver WebDriver { get; set; }

        public static string WebDriverPathForExecutable(string executableName)
        {
            if (executableName == null)
            {
                throw new NullReferenceException("The executable name parameter must be set.");
            }

            executableName = executableName.Replace(".exe", string.Empty, StringComparison.CurrentCulture);
            string driverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            FileInfo[] file = Directory.GetParent(driverPath).GetFiles($"{executableName}.exe", SearchOption.AllDirectories);
            var info = file.Length != 0 ? file[0].DirectoryName : driverPath;
            return info;
        }

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
        public void SetUpProjectSpecificConfiguration()
        {
            //Can remove
        }

        [BeforeScenario(Order = 3)]
        public void SetupWebDriver()
        {
            var browser = this.context.GetConfiguration<AppSettings>().Data.ContactUsConfig.Browser;
            var webDriver = new WebDriverConfigurator(browser).Create();

            if (browser.IsFirefox())
            {
                this.WebDriver = new FirefoxDriver(WebDriverPathForExecutable("geckodriver"));
            }
            else if (browser.IsChrome() || browser.IsZap() || browser.IsChromeHeadless())
            {
                var chromeDriverpath = WebDriverPathForExecutable("chromedriver");
                var chromeOptions = new ChromeOptions();

                if (browser.IsChrome())
                {
                    chromeOptions.AddArgument("no-sandbox");
                }
                else if (browser.IsChromeHeadless())
                {
                    chromeOptions.AddArgument("--headless");
                }
                else if (browser.IsZap())
                {
                    const string proxyUri = "localhost:8080";
                    var proxy = new Proxy
                    {
                        HttpProxy = proxyUri,
                        SslProxy = proxyUri,
                        FtpProxy = proxyUri,
                    };
                    chromeOptions.Proxy = proxy;
                }

                var commandTimeoutInMinutes = TimeSpan.FromMinutes(this.context.Get<FrameworkConfig>().TimeOutConfig.CommandTimeout);
                this.WebDriver = new ChromeDriver(chromeDriverpath, chromeOptions, commandTimeoutInMinutes);
            }
            else if (browser.IsIe())
            {
                this.WebDriver = new InternetExplorerDriver(WebDriverPathForExecutable("IEDriverServer"));
            }
            else if (browser.IsCloudExecution())
            {
                var browserStackSetting = this.objectContext.Get<FrameworkConfig>().BrowserStackSetting;
                this.objectContext.Get<FrameworkConfig>().BrowserStackSetting.Name = this.context.ScenarioInfo.Title;
                this.WebDriver = BrowserStackSetup.Init(browserStackSetting, this.objectContext.Get<EnvironmentConfig>());
            }
            else
            {
                throw new Exception($"The browser configuration ({browser}) is unrecognised. Please review your app settings.");
            }

            this.WebDriver.Manage().Window.Maximize();
            this.WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(this.context.Get<FrameworkConfig>().TimeOutConfig.PageNavigation);
            var currentWindow = this.WebDriver.CurrentWindowHandle;
            this.WebDriver.SwitchTo().Window(currentWindow);
            this.WebDriver.Manage().Cookies.DeleteAllCookies();

            if (!browser.IsCloudExecution())
            {
                var remoteWebDriver = this.WebDriver as RemoteWebDriver;
                var capabilities = remoteWebDriver.Capabilities;
                this.objectContext.SetBrowserName(capabilities["browserName"]);
                this.objectContext.SetBrowserVersion(capabilities["browserVersion"]);
            }

            this.context.SetWebDriver(this.WebDriver);
        }

        [BeforeScenario(Order = 4)]
        public void SetUpHelpers()
        {
            var webDriver = this.context.GetWebDriver();
            var webDriverwaitHelper = new WebDriverWaitHelper(webDriver, this.context.Get<FrameworkConfig>().TimeOutConfig);
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

        [BeforeScenario(Order = 5)]
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

            this.objectContext.SetDirectory(directory);
        }
    }
}
