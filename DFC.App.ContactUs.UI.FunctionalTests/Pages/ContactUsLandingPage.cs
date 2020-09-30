// <copyright file="ContactUsLandingPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Config;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.Pages
{
    internal class ContactUsLandingPage : BasePage
    {
        private readonly ScenarioContext context;
        private readonly IConfigurator<AppSettings> config;
        private readonly IWebDriver webDriver;

        public ContactUsLandingPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us home page cannot be initialised.");
            }

            this.webDriver = context.GetWebDriver();
            this.config = context.GetConfiguration<AppSettings>();
        }

        protected override string PageTitle => "Contact us";

        public ContactUsLandingPage NavigateToContactUsPage()
        {
            this.webDriver.Url = this.config.Data.ProjectConfiguration.BaseUrl + "/contact-us";
            return this;
        }
    }
}
