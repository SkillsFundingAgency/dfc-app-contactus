// <copyright file="ReportATechnicalIssueSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SFA.DFC.ContactUs.UITests.Project.Tests.Pages;
using TechTalk.SpecFlow;

namespace SFA.DFC.ContactUs.UITests.Project.Tests.StepDefinitions
{
    [Binding]
    public class ReportATechnicalIssueSteps
    {
        private readonly ScenarioContext context;
        private ReportATechnicalIssuePage reportATechnicalIssuePage;
        private EnterDetailsPage enterDetailsPage;

        public ReportATechnicalIssueSteps(ScenarioContext context)
        {
            this.context = context;
            this.reportATechnicalIssuePage = new ReportATechnicalIssuePage(this.context);
        }

        [When(@"I complete the first technical form with '(.*)' query")]
        public void WhenICompleteTheFirstTechnicalFormWithQuery(string query)
        {
            this.enterDetailsPage = this.reportATechnicalIssuePage
                    .EnterTechnicalQuery(query)
                    .ClickContinueonTechnicalForm()
                    .VerifyDetailsPage();
        }
    }
}