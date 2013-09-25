﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18051
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace CatBrows.Generator.Tests
{
    using TechTalk.SpecFlow;
    using System.Configuration;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("BrowserTest")]
    public partial class BrowserTestFeature
    {
        
        private string Browser;
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "BrowserTest.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BrowserTest", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
            this.Browser = null;
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            ScenarioContext.Current.Add("Browser", this.Browser);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 6
#line 7
 testRunner.Given("I have a browser when running the background", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        private void GuardBrowserTagMissing()
        {
            var enforceExistenceOfBrowserTagRaw = ConfigurationManager.AppSettings["CatBrowsEnforcesExistenceOfBrowserTag"];
            bool enforceExistenceOfBrowserTag;
            if (bool.TryParse(enforceExistenceOfBrowserTagRaw, out enforceExistenceOfBrowserTag) && enforceExistenceOfBrowserTag)
            {
                if (string.IsNullOrEmpty(this.Browser))
                {
                    throw new System.Exception("No browser defined, please specify @Browser:someBrowser for your scenario.");
                }
            }
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.IgnoreAttribute()]
        public virtual void NoTagsAtAll()
        {
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No tags at all", new string[] {
                        "ignore"});
#line 10
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 11
 testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute("SomeTag")]
        public virtual void TagsButNoBrowserTag()
        {
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Tags but no browser tag", new string[] {
                        "SomeTag",
                        "ignore"});
#line 15
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 16
 testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseAttribute("chrome", Description="Single browser tag chrome (chrome)", Category="chrome")]
        public virtual void SingleBrowserTagChrome(string browser)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Single browser tag chrome", new string[] {
                        "Browser:chrome"});
#line 19
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 20
 testRunner.Then("the test method should have 1 testcase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseAttribute("firefox", Description="Multiple browser tags (firefox)", Category="firefox")]
        [NUnit.Framework.TestCaseAttribute("chrome", Description="Multiple browser tags (chrome)", Category="chrome")]
        public virtual void MultipleBrowserTags(string browser)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Multiple browser tags", new string[] {
                        "Browser:firefox",
                        "Browser:chrome"});
#line 24
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 25
 testRunner.Then("the test method should have 2 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.TestCaseAttribute("value", null)]
        [NUnit.Framework.TestCaseAttribute("other value", null)]
        public virtual void NoTagsScenarioOutline(string header, string[] exampleTags)
        {
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "ignore"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No tags scenario outline", @__tags);
#line 28
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 29
 testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute("sometag")]
        [NUnit.Framework.TestCaseAttribute("value", null)]
        [NUnit.Framework.TestCaseAttribute("other value", null)]
        public virtual void TagsButNoBrowserTagScenarioOutline(string header, string[] exampleTags)
        {
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "sometag",
                    "ignore"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Tags but no browser tag scenario outline", @__tags);
#line 37
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 38
 testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseAttribute("scenario-outline-browser", "value", null, Description="scenario outline with single browser tag (scenario-outline-browser)", Category="scenario-outline-browser")]
        public virtual void ScenarioOutlineWithSingleBrowserTag(string browser, string header, string[] exampleTags)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "Browser:scenario-outline-browser"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("scenario outline with single browser tag", @__tags);
#line 45
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 46
 testRunner.Then("the test method should have 1 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseAttribute("chrome", "value", null, Description="scenario outline with two browser tags (chrome)", Category="chrome")]
        [NUnit.Framework.TestCaseAttribute("firefox", "value", null, Description="scenario outline with two browser tags (firefox)", Category="firefox")]
        public virtual void ScenarioOutlineWithTwoBrowserTags(string browser, string header, string[] exampleTags)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "Browser:chrome",
                    "Browser:firefox"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("scenario outline with two browser tags", @__tags);
#line 53
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 54
 testRunner.Then("the test method should have 2 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("OutlineTag")]
        [NUnit.Framework.TestCaseAttribute("chrome", "nightly", new string[] {
                "nightly"}, Description="scenario outline with two browser tags and tagged examples (chrome)", Category="nightly,chrome")]
        [NUnit.Framework.TestCaseAttribute("firefox", "nightly", new string[] {
                "nightly"}, Description="scenario outline with two browser tags and tagged examples (firefox)", Category="nightly,firefox")]
        [NUnit.Framework.TestCaseAttribute("chrome", "each-commit", new string[] {
                "each-commit"}, Description="scenario outline with two browser tags and tagged examples (chrome)", Category="each-commit,chrome")]
        [NUnit.Framework.TestCaseAttribute("firefox", "each-commit", new string[] {
                "each-commit"}, Description="scenario outline with two browser tags and tagged examples (firefox)", Category="each-commit,firefox")]
        public virtual void ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples(string browser, string header, string[] exampleTags)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "Browser:chrome",
                    "Browser:firefox",
                    "OutlineTag"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("scenario outline with two browser tags and tagged examples", @__tags);
#line 63
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 64
 testRunner.Then("the test method should have 4 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
