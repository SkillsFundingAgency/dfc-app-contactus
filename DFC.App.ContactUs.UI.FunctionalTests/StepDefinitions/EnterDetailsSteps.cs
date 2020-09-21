// <copyright file="EnterDetailsSteps.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SFA.DFC.ContactUs.UITests.Project.Tests.Pages;
using TechTalk.SpecFlow;

namespace SFA.DFC.ContactUs.UITests.Project.Tests.StepDefinitions
{
    [Binding]
    public class EnterDetailsSteps
    {
        private readonly ScenarioContext context;
        private EnterDetailsPage enterDetailsPage;
        private ConfirmationPage confirmationPage;

        public EnterDetailsSteps(ScenarioContext context)
        {
            this.context = context;
            this.enterDetailsPage = new EnterDetailsPage(this.context);
        }

        [When(@"I complete the form with details '(.*)','(.*)','(.*)','(.*)','(.*)'")]
        public void WhenICompleteTheFormWithDetails(string fname, string email, string confemail, string dob, string postcode)
        {
            this.enterDetailsPage.CompleteForm(fname, email, confemail, dob, postcode);
            this.enterDetailsPage.SelectTermsandConditions();
        }

        [When(@"I click send with nothing selected on the second form")]
        public void WhenIClickSendWithNothingSelectedOnTheSecondForm()
        {
            this.enterDetailsPage.ClickSendWithError();
        }

        [When(@"I click send")]
        public void WhenIClickSend()
        {
            this.confirmationPage = this.enterDetailsPage.ClickSend();
        }

        [When(@"I complete the feedback form with details '(.*)','(.*)','(.*)'")]
        public void WhenICompleteTheFeedbackFormWithDetails(string fname, string email, string confemail)
        {
            this.enterDetailsPage.CompleteFeedbackForm(fname, email, confemail);
            this.enterDetailsPage.SelectTermsandConditions();
        }

        [When(@"I select '(.*)' for additional contact")]
        public void WhenISelectForAdditionalContact(string consent)
        {
            this.enterDetailsPage.SelectAddContact(consent);
        }

        [Then(@"I am directed to the confirmation page")]
        public void ThenIAmDirectedToTheConfirmationPage()
        {
            this.confirmationPage.VerifyConfirmPage();
        }

        [Then(@"an error message is displayed on the second form")]
        public void ThenAnErrorMessageIsDisplayedOnTheSecondForm()
        {
            this.enterDetailsPage.VerifyErrorMessages();
        }

        [Then(@"a date of birth error is displayed")]
        public void ThenADateOfBirthErrorIsDisplayed()
        {
            this.enterDetailsPage.ClickSendWithError();
            this.enterDetailsPage.VerifyBirthDateErrorMessage();
        }
    }
}
