// <copyright file="ContactUsLandingPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.Pages
{
    internal class ContactUsLandingPage
    {
        private readonly ScenarioContext context;

        public ContactUsLandingPage(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us home page cannot be initialised.");
            }
        }

        public ContactUsLandingPage NavigateToContactUsPage()
        {
            this.context.GetWebDriver().Url = this.context.GetSettingsLibrary<AppSettings>().AppSettings.AppUrl.ToString();

            this.context.GetHelperLibrary().AxeHelper.Analyse();

            return this;
        }
    }
}
