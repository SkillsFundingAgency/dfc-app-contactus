Feature: Contact us by email

@ContactUs @Smoke
Scenario: Submit a careers advice and guidance email
	Given I am on the contact us landing page
	When I select the radio button option By email
	And I click the continue button
	Then I am on the How can we help? page
	When I select the radio button option <Careers advice and guidance>
	And I enter <random text> in the <Tell us why you want to contact us in as much detail as you can. Don't include any personal or account information.> field
	And I click the <continue> button