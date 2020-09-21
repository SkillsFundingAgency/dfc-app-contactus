// <copyright file="SelectAnOptionSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.UI.FunctionalTests.Pages;
using DFC.TestAutomation.UI.TestSupport;
using System;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    public class SelectAnOptionSteps
    {
        private readonly ScenarioContext context;
        private readonly ObjectContext objectContext;
        private ContactUsHomePage contactUsHomePage;
        private SelectAnOptionPage selectAnOptionPage;
        private FirstContactFormPage firstContactFormPage;
        private ReportATechnicalIssuePage reportaTechnicalIssuePage;

        public SelectAnOptionSteps(ScenarioContext context)
        {
            this.context = context;

            if (this.context == null)
            {
                throw new NullReferenceException("The scenario context is null. The select an option step definitions cannot be initialised.");
            }

            this.objectContext = this.context.Get<ObjectContext>();
            this.contactUsHomePage = new ContactUsHomePage(this.context);
            this.selectAnOptionPage = new SelectAnOptionPage(this.context);
        }

        [Given(@"I have selected '(.*)' option to continue onto the first contact form")]
        public void GivenIHaveSelectedOptionToContinueOntoTheFirstContactForm(string option)
        {
            this.selectAnOptionPage = this.contactUsHomePage
                .NavigateToContactUsPage()
                .ClickOnlineMessageLink()
                .SelectContactOption(option);

            if (this.objectContext.Get("SelectOption") != "Report a technical issue")
            {
                this.firstContactFormPage = this.selectAnOptionPage.ClickContinue();
            }
            else
            {
                this.reportaTechnicalIssuePage = this.selectAnOptionPage.ClickContinueTechnical();
            }
        }

        [Then(@"I am directed to the first contact form")]
        public void ThenIAmDirectedToTheFirstContactForm()
        {
            this.firstContactFormPage.VerifyQueryPage();
        }

        [Then(@"I am redirected to the first technical contact form")]
        public void ThenIAmRedirectedToTheFirstTechnicalContactForm()
        {
            this.reportaTechnicalIssuePage.VerifyTechnicalPage();
        }
    }
}
