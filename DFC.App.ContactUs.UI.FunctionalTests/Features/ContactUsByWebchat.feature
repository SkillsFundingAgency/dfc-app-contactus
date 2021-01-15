Feature: Contact us by webchat

@ContactUs
Scenario: Contact careers advice by web chat
	Given I am on the contact us landing page
	When I select the radio button option Chat online
	And I click the continue button
	Then I am taken to the webchat iFrame