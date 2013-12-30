Feature: BrowserRequiredTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
	Given I have a browser when running the background

@ignore
Scenario: No tags at all
	Then the test method should throw a no browser exception

@SomeTag
@ignore
Scenario: Tags but no browser tag
	Then the test method should throw a no browser exception

@Browser:chrome
Scenario: Single browser tag chrome
	Then the test method should have 1 testcase

@Browser:firefox
@Browser:chrome
@Duplicate:Property
@Duplicate:Property
Scenario: Multiple browser tags
	Then the test method should have 2 testcases

@ignore
@:
@a:
@:a
Scenario Outline: No tags scenario outline
	Then the test method should throw a no browser exception
Examples:
| header      |
| value       |
| other value |

@sometag
@ignore
Scenario Outline: Tags but no browser tag scenario outline
	Then the test method should throw a no browser exception
Examples:
| header      |
| value       |
| other value |

@Browser:scenario-outline-browser
Scenario Outline: scenario outline with single browser tag
	Then the test method should have 1 testcases
Examples:
| header      |
| value       |

@Browser:chrome
@Browser:firefox
Scenario Outline: scenario outline with two browser tags
	Then the test method should have 2 testcases
Examples:
| header      |
| value       |
| other value |

@Browser:chrome
@Browser:firefox
@OutlineTag
@CustomProperty:PropertyValue
Scenario Outline: scenario outline with two browser tags and tagged examples
	Then the test method should have 4 testcases
@nightly
Examples:
| header  |
| nightly |
@each-commit
Examples:
| header      |
| each-commit |