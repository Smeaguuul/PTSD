Feature: MatchPoints

A feature that tests the method that keeps tracks of the point logic. 
Stuff like: When to move on to a new set? When has a team won? And so on.

@tag1
  Scenario: Start Game
    Given Team A has 0 points won
    And Team B has 0 points won
    When Team A wins the next 2 points
    Then The point count for Team A should be 30
    And The point count for Team B should be 0


#  Scenario: Move to new game
#    Given Team A has 3 points won
#    And Team B has 3 points won
#    And the current set score is 0-0
#    When Team A wins the next two points
#    Then the match should move to a new game
#    And the set count for Team A should remain 0
#    And the set count for Team B should be updated to 1
#
#
#  Scenario: Move to a new set
#    Given Team A has 1 set won
#    And Team B has 0 set won
#    And the current set score is 5-4 in favor of Team B
#    When Team B wins the next game
#    Then the match should move to the next set
#    And the current set score should be reset to 0-0
#    And the set count for Team A should remain 1
#    And the set count for Team B should be updated to 1


