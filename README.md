CatBrows
======

CatBrows is a SpecFlow generator plugin that adds @Browser:browser tags to scenarios. Actually, when using this generator you are forced to add a Browser-tag or else your scenario will throw an Exception (unless configured not to).

You can specify multiple browsers per scenario or scenario outline and each will be run separately. 

This plugin does NOT hook up with Selenium or any other browser driver, have a look at https://github.com/baseclass/Contrib.SpecFlow.Selenium.NUnit if this is what you need. 

CatBrows will add a {"Browser", "some browser"} key-value to ScenarioContext.Current and add a TestCaseAttribute for each @Browser:wanted-browser tag. You have to take care of the driver hookup yourself in a, for example, separate BeforeScenario-method.

```Cucumber
@Browser:chrome
@Browser:firefox
Scenario: Run same test case with multiple browsers
  Given I have a web test case
  When I run my Browser-tagged scenario
  Then the scenario should be run for both firefox and chrome
```  
Will generate (not really but almost):
```C#
[TestCase("chrome")]
[TestCase("firefox")]
public void RunSameTestCaseWithMultipleBrowsers(string browser)
{
  ScenarioContext.Current.Add("Browser", browser);
  /* 
    ... Specflow's standard content goes here.
  */
}
```
  
  
```Cucumber  
Scenario: Run a test without a Browser tag
  Given I have a scenario without a Browser tag
  When I run my scenario
  Then the test run should explode since I did not provide a Browser tag
```
yields
```C#
[Test]
public void RunATestWithoutABrowserTag()
{
  throw new Exception("No browser defined, please specify @Browser:someBrowser for your scenario.")
}
```
The forced @Browser:browser tag check can be removed by specifying the following in your App.config:
```xml
  <appSettings>
    <add key="CatBrows-RequiresBrowser" value="false" />
  </appSettings>
```
Custom browser missing messages are specified with the setting:
```xml
  <appSettings>
    <add key="CatBrows-RequiresBrowser" value="true" /> <!-- defaults to true -->
    <add key="CatBrows-BrowserMissingMessage" value="Oh noes, there is no @Browser-tag present." />
  </appSettings>
```

To repeat a specific scenario multiple times, use the @Repeat:<int> tag

```Cucumber
@Repeat:3
Scenario: Run a scenario multiple times
  Given I have a scenario with a repeat tag
  When I run my Repeat-tagged scenario with repeats x 3
  Then the scenario should be run three times
```
