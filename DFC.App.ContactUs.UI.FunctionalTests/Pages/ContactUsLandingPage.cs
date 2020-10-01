// <copyright file="ContactUsLandingPage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.Model;
using DFC.TestAutomation.UI.Extension;
using DFC.TestAutomation.UI.TestSupport;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.Pages
{
    internal class ContactUsLandingPage : BasePage
    {
        private readonly ScenarioContext context;

        public ContactUsLandingPage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us home page cannot be initialised.");
            }
        }

        protected override string PageTitle => "Contact us";

        public ContactUsLandingPage NavigateToContactUsPage()
        {
            this.context.GetWebDriver().Url = this.context.GetConfiguration<AppSettings>().Data.ProjectConfiguration.AppUrl.ToString();
            return this;
        }
    }
}
