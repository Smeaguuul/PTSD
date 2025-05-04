Feature: MatchPoints

A feature that tests the method that keeps tracks of the point logic. 
Stuff like: When to move on to a new set? When has a team won? And so on.

@tag1
  Scenario: Team A Wins Two Points
    Given Team A has 0 points won
    And Team B has 0 points won
    When Team A wins the next 2 points
    Then The point count for Team A should be 30
    And The point count for Team B should be 0

@tag1
  Scenario: Both Go To 40 Points
    Given Team A has 0 points won
    And Team B has 0 points won
    When Team A wins the next 3 points
    And Team B wins the next 3 points
    Then The point count for Team A should be 40
    And The point count for Team B should be 40

@tag1
  Scenario: Both Have 40 Points, And a Team Scores
    Given Team A has 3 points won
    And Team B has 3 points won
    When Team A wins the next 1 points
    Then The point count for Team A should be Advantage
    And The point count for Team B should be 40

@tag1
  Scenario: Move to new game
    Given Team A has 3 points won
    And Team B has 3 points won
    When Team A wins the next 2 points
    Then the match should move to a new game
    And the set count for Team A should be 1
    And the set count for Team B should be 0

@tag1
  Scenario: Move to a new set
    Given Team A has 0 set won
    And Team B has 0 set won
    And the current set score is 5-4 in favor of Team A
    When Team A wins the next 4 points
    Then the match should move to the next set
    And the current set score should be reset to 0-0
    And the won set count for Team A should be 1
    And the won set count for Team B should be 0


