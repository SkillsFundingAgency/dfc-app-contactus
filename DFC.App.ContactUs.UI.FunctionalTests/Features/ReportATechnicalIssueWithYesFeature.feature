Feature: Report a Technical Issue with Yes in Additonal Contact
@Contactus
@Smoke
Scenario: Report a Technical Issue wth Yes for additonal contact
	Given I have selected 'Report a technical issue' option to continue onto the first contact form
	Then I am redirected to the first technical contact form
	When I complete the first technical form with 'Unable to access website' query
   		And I complete the feedback form with details 'Automated Test','automatedtestesfa@mailinator.com','automatedtestesfa@mailinator.com'
		And I select 'Yes' for additional contact
		And I click send
	Then I am directed to the confirmation page