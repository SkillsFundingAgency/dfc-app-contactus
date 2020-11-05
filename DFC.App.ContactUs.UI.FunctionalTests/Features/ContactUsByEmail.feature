Feature: Contact us by email

@ContactUs @Smoke
Scenario: Submit a careers advice and guidance email
	Given I am on the contact us landing page
	When I select the radio button option By email
	And I click the continue button
	Then I am on the How can we help? page
	When I select the radio button option Careers advice and guidance
	And I enter Hello. Please provide me with advice. in the Tell us why you want to contact us in as much detail as you can. Don’t include any personal or account information. field
	And I click the next button
	Then I am on the Enter your details page
	When I enter James in the First name field
	And I enter Barnes in the Last name field
	And I enter james.barnes@email.com in the Email address field
	And I enter 07737292448 in the Telephone number field
	And I enter 1 in the Day field
	And I enter 1 in the Month field
	And I enter 1980 in the Year field
	And I enter CV7 7RE in the Postcode field
	And I click the checkbox option I accept the terms and conditions
	And I click the send button
	Then I am on the Thank you for contacting us page