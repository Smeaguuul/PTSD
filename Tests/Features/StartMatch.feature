Feature: StartMatch

A match is started in the admin panel, and an enduser ends with the pointmanager screen.

@MatchManagement
Scenario: Admin Logs In and Starts Game
  Given I navigate to the login page
  When I enter my credentials
    | Field    | Value           |
    | Username | admin           |
    | Password | 1234			 |
  And I log in
  Then I should be on the dashboard
  When I navigate to the start match page
  And I select a field to start a game
  And I select a match to start
  Then I should receive a QR code that refers to the point manager

@MatchManagement
Scenario: Admin Starts a Game
  Given I am on the admin dashboard and logged in
  When I navigate to the start match page
  And I select a field to start a game
  And I select a match to start
  Then I should receive a QR code that refers to the point manager

@MatchManagement
Scenario: End User Scans QR Code and Starts Game
	Given Given the end user has a QR code for the point manager of a new match
	When the end user scans the QR code and enters the link
	Then the first view should be the selection of who serves first
	And the next view should be the point manager
