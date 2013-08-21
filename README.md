CatBrows
======

CatBrows is a SpecFlow generator plugin that adds @Browser:browser tags to scenarios. Actually, when using this generator you are forced to add a Browser-tag or else your scenario will throw an NoBrowserDefinedException.

You can specify multiple browsers per scenario or scenario outline and each will be run separately. 

This plugin does NOT hook up with Selenium or any other browser driver, have a look at https://github.com/baseclass/Contrib.SpecFlow.Selenium.NUnit if this is what you need. 

CatBrows will add a {"Browser", "some browser"} key-value to ScenarioContext.Current and add a TestCaseAttribute for each @Browser:wanted-browser tag. You have to take care of the driver hookup yourself in a, for example, separate BeforeScenario-method.

```Cucumber
@Browser:chrome
@Browser:firefox
Scenario: Run same test case with multiple browser
  Given I have a web test case
  When I run my Browser-tagged scenario
  Then the scenario should be run for both firefox and chrome
  
  
Scenario: Run a test without a Browser tag
  Given I have a scenario without a Browser tag
  When I run my scenario
  Then the test run should explode since I did not provide a Browser tag
  
```  

