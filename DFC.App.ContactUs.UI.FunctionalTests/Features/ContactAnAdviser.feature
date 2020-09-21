Feature: ContactAnAdviser	

@Contactus
@Smoke
Scenario: Contact an Adviser
	Given I have selected 'Contact an adviser' option to continue onto the first contact form
	Then I am directed to the first contact form
	When I complete the first form with 'Funding' option and 'Contact an Adviser Form' query
		And I complete the form with details 'Automated Test','automatedtestesfa@mailinator.com','automatedtestesfa@mailinator.com','20/11/2000','CV3 5FE'
		And I click send
	Then I am directed to the confirmation page
