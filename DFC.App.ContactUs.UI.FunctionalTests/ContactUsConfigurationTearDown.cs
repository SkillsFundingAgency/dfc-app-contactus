// <copyright file="ContactUsConfigurationTearDown.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CsvHelper;
using CsvHelper.Configuration;
using DFC.TestAutomation.UI.Config;
using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs
{
    [Binding]
    public class ContactUsConfigurationTearDown
    {
        private readonly ScenarioContext context;
        private readonly BrowserHelper browserHelper;

        private ObjectContext objectContext;

        public ContactUsConfigurationTearDown(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The configuration set up cannot be initialised.");
            }

            this.objectContext = this.context.Get<ObjectContext>();
            this.WebDriver = this.context.GetWebDriver();
            this.browserHelper = new BrowserHelper(this.context.GetConfiguration().Data.BrowserConfiguration.BrowserName);
        }

        private IWebDriver WebDriver { get; set; }

        [AfterScenario(Order = 0)]
        public void CollectTestData()
        {
            try
            {
                this.objectContext.SetAfterScenarioExceptions(new List<Exception>());

                var fileName = DateTime.Now.ToString("HH-mm-ss", CultureInfo.CurrentCulture)
                       + "_"
                       + this.context.ScenarioInfo.Title
                       + ".txt";

                var filePath = Path.Combine(this.objectContext.GetDirectory(), fileName);
                var records = new List<TestData>();
                var allTestData = this.objectContext.GetAll();
                allTestData.ToList().ForEach(x => records.Add(new TestData { Key = x.Key, Value = allTestData[x.Key].ToString() }));

                using (var writer = new StreamWriter(filePath))
                {
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture)))
                    {
                        csv.WriteRecords(records);
                        writer?.Flush();
                    }
                }

                TestContext.AddTestAttachment(filePath, fileName);
            }
            catch (Exception ex)
            {
                this.objectContext.SetAfterScenarioException(ex);
            }
        }

        [AfterScenario(Order = 1)]
        public void TakeScreenshotOnFailure()
        {
            if (this.context.TestError != null)
            {
                try
                {
                    var scenarioTitle = this.context.ScenarioInfo.Title;
                    var webDriver = this.context.GetWebDriver();
                    var directory = this.objectContext.GetDirectory();

                    this.objectContext.SetUrl(webDriver.Url);

                    ScreenshotHelper.TakeScreenShot(webDriver, directory, scenarioTitle, true);
                }
                catch (Exception ex)
                {
                    this.objectContext.SetAfterScenarioException(ex);
                }
            }
        }

        [AfterScenario(Order = 2)]
        public void InformBrowserStackOnFailure()
        {
            if (this.context.TestError != null)
            {
                var browser = this.objectContext.GetBrowser();
                var webDriver = this.context.GetWebDriver();
                var errorMessage = this.context.TestError.Message;

                switch (true)
                {
                    case bool _ when this.browserHelper.IsExecutingInTheCloud():
                        try
                        {
                            RemoteWebDriver remoteWebDriver = (RemoteWebDriver)webDriver;
                            var sessionId = remoteWebDriver.SessionId.ToString();
                            BrowserStackReport.MarkTestAsFailed(this.context.GetConfiguration().Data.BrowserStackConfiguration, sessionId, errorMessage);
                        }
                        catch (Exception ex)
                        {
                            this.objectContext.SetBrowserstackResponse();
                            this.objectContext.SetAfterScenarioException(ex);
                        }

                        break;
                }
            }
        }

        [AfterScenario(Order = 3)]
        public void DisposeWebDriver()
        {
            try
            {
                var disposeWebDriver = this.context.TestError == null && !this.browserHelper.IsExecutingInTheCloud() && !this.objectContext.FailedtoUpdateTestResultInBrowserStack();

                if (disposeWebDriver)
                {
                    this.WebDriver?.Quit();
                    this.WebDriver?.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.objectContext.SetAfterScenarioException(ex);
            }
        }

        [AfterScenario(Order = 4)]
        public void ReportErrorMessages()
        {
            var exception = this.context.TestError;

            if (exception != null)
            {
                var messages = new List<string>();

                messages.AddRange(this.objectContext.GetAfterScenarioExceptions().Select(x => x.Message));

                var url = this.objectContext.GetUrl();

                if (!string.IsNullOrEmpty(url))
                {
                    messages.Add($"Url : {url}");
                }

                throw new Exception(string.Join(Environment.NewLine, messages));
            }
        }
    }
}
