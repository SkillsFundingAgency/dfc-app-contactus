// <copyright file="AfterScenario.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium.Remote;
using System;
using System.Threading.Tasks;
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
                throw new NullReferenceException($"The scenario context is null. The {this.GetType().Name} class cannot be initialised.");
            }
        }

        private ScenarioContext Context { get; set; }

        [AfterScenario(Order = 0)]
        public async Task UpdateBrowserStack()
        {
            var browserHelper = this.Context.GetHelperLibrary<AppSettings>().BrowserHelper;

            if (browserHelper.IsExecutingInBrowserStack() && this.Context.TestError != null)
            {
                var sessionId = (this.Context.GetWebDriver() as RemoteWebDriver).SessionId.ToString();
                var errorMessage = this.Context.TestError.Message;
                var browserStackHelper = this.Context.GetHelperLibrary<AppSettings>().BrowserStackHelper;
                await browserStackHelper.SetTestToFailedWithReason(sessionId, errorMessage).ConfigureAwait(false);
            }
        }

        [AfterScenario(Order = 1)]
        public void DisposeWebDriver()
        {
            var webDriver = this.Context.GetWebDriver();
            webDriver?.Quit();
            webDriver?.Dispose();
        }
    }
}