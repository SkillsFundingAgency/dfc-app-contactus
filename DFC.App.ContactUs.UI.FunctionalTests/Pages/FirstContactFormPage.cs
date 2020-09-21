using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using FluentAssertions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
namespace SFA.DFC.ContactUs.UITests.Project.Tests.Pages
{
    public class FirstContactFormPage : BasePage 
    {
        #region Helpers
        private readonly PageInteractionHelper _pageHelper;
        private readonly FormCompletionHelper _formHelper;
        private readonly ScenarioContext _context;
        private readonly ObjectContext _objectContext;
        #endregion
        #region Page Elements
        protected override string PageTitle => "";
        private By QueryPageTitle = By.ClassName("govuk-fieldset__heading");
        private By Message = By.Id("Message");
        private By Feedback = By.Id("Feedback");
        private By ContinueButton = By.Id("send-feedback-details");
        private By CategoryErrorMessage = By.LinkText("Choose a category");
        private By IssueErrorMessage = By.Id("Message-error");
        private List<IWebElement> OptionsList => _pageHelper.FindElements(By.ClassName("govuk-radios__input"));
        #endregion
        public FirstContactFormPage(ScenarioContext context): base(context)
        {
            _context = context;
            _pageHelper = context.Get<PageInteractionHelper>();
            _formHelper = context.Get<FormCompletionHelper>();
            _objectContext = context.Get<ObjectContext>();
        }
        public void VerifyQueryPage()
        {
            if (_objectContext.Get("SelectOption") == "Contact an adviser")
            {
                _pageHelper.VerifyText(QueryPageTitle, "What is your query about?").Should().BeTrue();
            }
            else if (_objectContext.Get("SelectOption") == "Give feedback")
            {
                _pageHelper.VerifyText(QueryPageTitle, "What is your feedback about?").Should().BeTrue();
            }
            
        }
      public FirstContactFormPage SelectQueryOption(string strOption)
        {
            {
                if (!string.IsNullOrWhiteSpace(strOption))
                {
                    var OptionText = strOption.Replace(" ", string.Empty).ToUpper();
                    foreach (var button in OptionsList)
                    {
                        var buttonText = button.GetAttribute("value").Replace(" ", string.Empty).ToUpper();
                        if (buttonText.Contains(OptionText))
                        {
                            button.Click();
                        }
                    }
                }
                return this;
            }
        }
        public FirstContactFormPage EnterQuery(string strQuery)
        {
            string option = _objectContext.Get("SelectOption");
            if(option.Equals("Contact an adviser",StringComparison.OrdinalIgnoreCase))
            {
                _formHelper.EnterText(Message, strQuery);
            }
            else if (option.Equals("Give feedback", StringComparison.OrdinalIgnoreCase))
            {
                _formHelper.EnterText(Feedback, strQuery);
            }
            return this;
        }
        public EnterDetailsPage ClickContinueFirstForm()
        {
            _formHelper.ClickElement(ContinueButton);
            return new EnterDetailsPage(_context);
        }
        public void VerifyErrorMessages()
        {
            _pageHelper.VerifyText(CategoryErrorMessage, "Choose a category").Should().BeTrue();
            _pageHelper.VerifyText(IssueErrorMessage, "Enter a message describing the issue").Should().BeTrue();
        }

        public FirstContactFormPage ClickContinueonError()
        {
            _formHelper.ClickElement(ContinueButton);
            return this;
        }
    }
}
