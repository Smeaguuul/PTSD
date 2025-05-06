Feature: Giveaway

A short summary of the feature

@GiveawayTag
Scenario: Create giveaway
	Given name is testGA 
	And a description is testGADesc
	And a start date is 2025-10-01
	And an end date is 2025-10-02
	When you create a giveaway
	Then the giveaway should be created

	@GiveawayTag2
	Scenario: Create giveaway with early date
	Given name is testGAEarly
	And a description is testGADescEarly
	And a start date is past
	And an end date is 2025-01-02
	When you create a giveaway with early date
	Then An error should be thrown with message "Start date cannot be in the past."


  @InvalidDates
  Scenario: Create giveaway with end date before start date
    Given name is InvalidGA
    And a description is Invalid Desc
    And a start date is 2025-12-01
    And an end date is 2025-11-30
    When you create a giveaway with end date before start date
    Then an error should be thrown with message "Start date cannot be after end date"

  @ContestantExists
  Scenario: Add existing contestant to giveaway
    Given a giveaway with name DupGA and active dates exists
    And a contestant with email "dup@example.com" and name "Bo" is added to a giveaway
    When you add the same contestant again
	Then it should return false and no contestant added

  @PickWinner
  Scenario: Pick one winner
    Given a giveaway with name WinGA and 3 contestants exists
    When you pick one winner
    Then one winner should be returned

  @RemoveContestant
  Scenario: Remove contestant from giveaway
    Given a giveaway with name RemoveGA and one contestant exists
    When you remove the contestant
    Then the contestant should be removed from the giveaway

