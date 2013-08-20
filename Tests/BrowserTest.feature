Feature: BrowserTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I have a browser when running the background

Scenario: No tags at all
	Then the test method should throw a no browser exception

@some-tag
Scenario: Tags but no browser tag
	Then the test method should throw a no browser exception

@Browser:chrome
Scenario: Single browser tag chrome
	Then the test method should have 1 testcase

@Browser:firefox
@Browser:chrome
Scenario: Multiple browser tags
	Then the test method should have 2 testcases


Scenario Outline: No tags scenario outline
	Then the test method should throw a no browser exception
Examples:
| header |
| value  |

@sometag
Scenario Outline: Tags but no browser tag scenario outline
	Then the test method should throw a no browser exception
Examples:
| header |
| value  |

@Browser:scenario-outline-browser
Scenario Outline: scenario outline with single browser tag
	Then the test method should throw a no browser exception
Examples:
| header |
| value  |

@Browser:chrome
@Browser:firefox
Scenario Outline: scenario outline with two browser tags
	Then the test method should throw a no browser exception
Examples:
| header |
| value  |