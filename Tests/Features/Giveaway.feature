Feature: Giveaway

A short summary of the feature

@tag1
Scenario: Create giveaway
	Given name is testGA 
	And a description is testGADesc
	And a start date is 2025-10-01
	And an end date is 2025-10-02
	When you create a giveaway
	Then the giveaway should be created
