using DFC.TestAutomation.UI.Helpers;
using DFC.TestAutomation.UI.TestSupport;
using OpenQA.Selenium;
using System;
using TechTalk.SpecFlow;


namespace SFA.DFC.ContactUs.UITests.Project.Tests.Pages
{
    public class SelectAnOptionPage : BasePage 
    {
        #region Helpers
        private readonly FormCompletionHelper _formHelper;
        private readonly ScenarioContext _context;
        private readonly ObjectContext _objectContext;
        #endregion
        #region Page Elements
        protected override string PageTitle => "";
        private By ContactAdviser = By.Id("ContactOptionType_ContactAdviser");
        private By TechnicalIssue = By.Id("ContactOptionType_Technical");
        private By Feedback = By.Id("ContactOptionType_Feedback");
        private By ContinueButton = By.Id("show-basic-details");
        
        #endregion 
        public SelectAnOptionPage(ScenarioContext context): base(context)
        {
            _context = context;
            _formHelper = context.Get<FormCompletionHelper>();
            _objectContext = context.Get<ObjectContext>();          
        }

        public SelectAnOptionPage SelectContactOption(string option)
        {
          
            if(option.Equals("Contact an adviser",StringComparison.OrdinalIgnoreCase))
            {
                _formHelper.SelectRadioButton(ContactAdviser);
            }
            else if (option.Equals("Report a technical issue",StringComparison.OrdinalIgnoreCase))
            {
                _formHelper.SelectRadioButton(TechnicalIssue);
            }
            else if (option.Equals("Give feedback",StringComparison.OrdinalIgnoreCase))
            {
                _formHelper.SelectRadioButton(Feedback);
            }
            _objectContext.Set("SelectOption", option);
            return this;
        }
        
        public FirstContactFormPage ClickContinue()
        {
             _formHelper.ClickElement(ContinueButton);
             return new FirstContactFormPage(_context);           
           
        }
        public ReportATechnicalIssuePage ClickContinueTechnical()
        {
            _formHelper.ClickElement(ContinueButton);
            return new ReportATechnicalIssuePage(_context);
        }
        
    }
}
