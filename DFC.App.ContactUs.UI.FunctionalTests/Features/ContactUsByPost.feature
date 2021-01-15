Feature: Contact us by post

@ContactUs
Scenario: Contact careers advice by post
	Given I am on the contact us landing page
	When I select the radio button option By post
	And I click the continue button
	Then I am on the Send us a letter page