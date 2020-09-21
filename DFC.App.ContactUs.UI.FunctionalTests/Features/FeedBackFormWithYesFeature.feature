Feature: Give Feedback with Yes in addtional contact
@Contactus
@Smoke
Scenario: Submit a Give Feedback form with Yes in additional contact 
	Given I have selected 'Give feedback' option to continue onto the first contact form
	Then I am directed to the first contact form
	When I complete the first form with 'Customer service' option and 'Contact for feedback' query
		And I complete the feedback form with details 'Automated Test','automatedtestesfa@mailinator.com','automatedtestesfa@mailinator.com'
		And I select 'Yes' for additional contact
		And I click send
	Then I am directed to the confirmation page