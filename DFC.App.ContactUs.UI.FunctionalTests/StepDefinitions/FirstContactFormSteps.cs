// <copyright file="FirstContactFormSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using DFC.App.ContactUs.UI.FunctionalTests.Pages;
using TechTalk.SpecFlow;

namespace DFC.App.ContactUs.UI.FunctionalTests.StepDefinitions
{
    [Binding]
    public class FirstContactFormSteps
    {
        private readonly ScenarioContext context;
        private FirstContactFormPage firstContactFormPage;
        private EnterDetailsPage enterDetailsPage;

        public FirstContactFormSteps(ScenarioContext context)
        {
            this.context = context;
            this.firstContactFormPage = new FirstContactFormPage(this.context);
        }

        [When(@"I complete the first form with '(.*)' option and '(.*)' query")]
        public void WhenICompleteTheFirstFormWithOptionAndQuery(string option, string message)
        {
            this.enterDetailsPage = this.firstContactFormPage
                    .SelectQueryOption(option)
                    .EnterQuery(message)
                    .ClickContinueFirstForm()
                    .VerifyDetailsPage();
        }

        [When(@"I press continue with nothing selected")]
        public void WhenIPressContinueWithNothingSelected()
        {
            this.firstContactFormPage.ClickContinueonError();
        }

        [Then(@"an error message is displayed on the first form")]
        public void ThenAnErrorMessageIsDisplayedOnTheFirstForm()
        {
            this.firstContactFormPage.VerifyErrorMessages();
        }
    }
}
