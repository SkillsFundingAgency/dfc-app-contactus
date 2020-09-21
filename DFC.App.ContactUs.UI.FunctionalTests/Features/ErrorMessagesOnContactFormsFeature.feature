Feature: ErrorMessagesOnContactFormsFeature	

@Contactus
@Smoke
Scenario: Error Messages on Contact Forms
	Given I have selected 'Contact an adviser' option to continue onto the first contact form
	Then I am directed to the first contact form
	When I press continue with nothing selected
	Then an error message is displayed on the first form
	When I complete the first form with 'Funding' option and 'Contact an Adviser Form' query
		And I click send with nothing selected on the second form
	Then an error message is displayed on the second form
	When I complete the form with details 'Automated Test','automatedtestesfa@mailinator.com','automatedtestesfa@mailinator.com','20/11/2018','CV3 5FE'
	Then a date of birth error is displayed
	When I complete the form with details 'Automated Test','automatedtestesfa@mailinator.com','automatedtestesfa@mailinator.com','20/11/2000','CV3 5FE'
		And I click send
	Then I am directed to the confirmation page