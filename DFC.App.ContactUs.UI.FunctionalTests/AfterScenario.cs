// <copyright file="AfterScenario.cs" company="National Careers Service">
// Copyright (c) National Careers Service. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using OpenQA.Selenium;
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
                throw new NullReferenceException($"The scenario context is null. The {this.GetType().Name} class cannot be initialised.");
            }
        }

        private ScenarioContext Context { get; set; }

        [AfterScenario(Order = 0)]
        public void TakeScreenshotOnFailure()
        {
            if (this.Context.TestError != null)
            {
                this.Context.GetHelperLibrary<AppSettings>().ScreenshotHelper.TakeScreenshot(this.Context);
            }
        }

        [AfterScenario(Order = 1)]
        public void DisposeWebDriver()
        {
            if (!this.Context.GetHelperLibrary<AppSettings>().BrowserHelper.IsExecutingInBrowserStack())
            {
                var webDriver = this.Context.GetWebDriver();
                webDriver?.Quit();
                webDriver?.Dispose();
            }
        }
    }
}
