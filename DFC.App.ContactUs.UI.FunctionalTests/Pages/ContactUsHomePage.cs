// <copyright file="ContactUsHomePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;

namespace SFA.DFC.ContactUs.UITests.Project.Tests.Pages
{
    public class ContactUsHomePage : BasePage
    {
        private readonly FormCompletionHelper formHelper;
        private readonly ScenarioContext context;
        private readonly ContactUs config;
        private readonly IWebDriver webDriver;
        private readonly By onlineMessageLink = By.LinkText("Send us an online message");

        public ContactUsHomePage(ScenarioContext context)
            : base(context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The contact us home page cannot be initialised.");
            }

            this.formHelper = this.context.Get<FormCompletionHelper>();
            this.webDriver = context.GetWebDriver();
            this.config = context.GetContactUsConfig<ContactUs>();
        }

        protected override string PageTitle => string.Empty;

        public ContactUsHomePage NavigateToContactUsPage()
        {
            this.webDriver.Url = this.config.BaseUrl + "/contact-us";
            return this;
        }

        public SelectAnOptionPage ClickOnlineMessageLink()
        {
            this.formHelper.ClickElement(this.onlineMessageLink);
            return new SelectAnOptionPage(this.context);
        }
    }
}
