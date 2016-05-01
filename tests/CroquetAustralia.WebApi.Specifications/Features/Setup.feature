Feature: Setup
	As a visitor
	I want to run the setup procedure
	So the site can be used

Scenario: Calling setup procedure for the first time
	Given the website is running and the setup procedure has not been run
    When I call /application/setup
    Then HTTP response status code should be NoContent
    And the setup procedure should run
    
Scenario: Calling setup procedure when setup has previously been called
	Given the website is running and the setup procedure has been run
    When I call /application/setup
    Then HTTP response status code should be TBD
    And HTTP response message should 'Setup cannot be repeated'