Feature: Manage companies in the system

	Scenario: Company gets created successfully
		When I create companies with the following details
			| Name              | Picture                     |
			| InsaneElectronics | insane_electronics_logo.csv |
			| Sentegral         | sentegral.jpg               |
		Then the companies are created successfully

	Scenario: Company gets deleted successfully
		Given the companies exist in the system
			| Name              | Picture                     |
			| InsaneElectronics | insane_electronics_logo.csv |
			| Sentegral         | sentegral.jpg               |
		When I delete the companies
		Then the companies are deleted