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
namespace TestSample.DefaultSettings
{
    using TechTalk.SpecFlow;
    using System.Configuration;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("BrowserRequiredTest")]
    [NUnit.Framework.CategoryAttribute("BrowserRequiredTestFeature")]
    [NUnit.Framework.CategoryAttribute("DefaultSettings")]
    public partial class BrowserRequiredTestFeature
    {
        
        private string Browser;
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        internal static object[] SingleBrowserTagChrome_chrome
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("chrome"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] MultipleBrowserTags_firefox
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("firefox"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] MultipleBrowserTags_chrome
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("chrome"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] Repeated3Times_browser
        {
            get
            {
                int repeats = 3;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("browser"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] Repeated3TimesWith2OutlineValues_browser
        {
            get
            {
                int repeats = 3;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("browser", "other value", null));
                rows.Add(new NUnit.Framework.TestCaseData("browser", "value", null));
                rows.Add(new NUnit.Framework.TestCaseData("browser"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] ScenarioOutlineWithSingleBrowserTag_scenariooutlinebrowser
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("scenario-outline-browser", "value", null));
                rows.Add(new NUnit.Framework.TestCaseData("scenario-outline-browser"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] ScenarioOutlineWithTwoBrowserTags_chrome
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("chrome", "other value", null));
                rows.Add(new NUnit.Framework.TestCaseData("chrome", "value", null));
                rows.Add(new NUnit.Framework.TestCaseData("chrome"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] ScenarioOutlineWithTwoBrowserTags_firefox
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("firefox", "other value", null));
                rows.Add(new NUnit.Framework.TestCaseData("firefox", "value", null));
                rows.Add(new NUnit.Framework.TestCaseData("firefox"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples_chrome
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("chrome", "each-commit", null));
                rows.Add(new NUnit.Framework.TestCaseData("chrome", "nightly", null));
                rows.Add(new NUnit.Framework.TestCaseData("chrome"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        internal static object[] ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples_firefox
        {
            get
            {
                int repeats = 1;
                System.Collections.Generic.List<NUnit.Framework.TestCaseData> rows = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                rows.Add(new NUnit.Framework.TestCaseData("firefox", "each-commit", null));
                rows.Add(new NUnit.Framework.TestCaseData("firefox", "nightly", null));
                rows.Add(new NUnit.Framework.TestCaseData("firefox"));
                int maxArguments = rows.Max(row => row.Arguments.Count());
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> filteredRows = rows.Where(row => row.Arguments.Count().Equals(maxArguments));
                System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData> repeatedRows = filteredRows.SelectMany(data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            });
                return repeatedRows.ToArray();
            }
        }
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "BrowserRequiredTest", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
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
            testRunner.Given("I have a browser when running the background", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
        }
        
        private void GuardBrowserTagMissing()
        {
            System.Collections.Specialized.NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
            string browserMissingMessage = "No browser defined, please specify @Browser:someBrowser for your scenario.";
            string customBrowserMissingMessage = appSettings.Get("CatBrows-BrowserMissingMessage");
            if ((customBrowserMissingMessage != null))
            {
                browserMissingMessage = customBrowserMissingMessage;
            }
            string enforceExistenceOfBrowserTagRaw = appSettings.Get("CatBrows-RequiresBrowser");
            bool enforceExistenceOfBrowserTag;
            bool hasConfigSetting = bool.TryParse(enforceExistenceOfBrowserTagRaw, out enforceExistenceOfBrowserTag);
            bool hasBrowser = (string.IsNullOrEmpty(this.Browser) != true);
            if ((string.IsNullOrEmpty(this.Browser) != true))
            {
                hasBrowser = true;
            }
            bool shouldGuard = !(hasConfigSetting && !enforceExistenceOfBrowserTag);
            if (shouldGuard)
            {
                if (!hasBrowser)
                {
                    throw new System.Exception(browserMissingMessage);
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
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseSourceAttribute("SingleBrowserTagChrome_chrome", Category="chrome")]
        public virtual void SingleBrowserTagChrome(string browser)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Single browser tag chrome", new string[] {
                        "Browser:chrome"});
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 1 testcase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("Duplicate:Property")]
        [NUnit.Framework.CategoryAttribute("Duplicate:Property")]
        [NUnit.Framework.TestCaseSourceAttribute("MultipleBrowserTags_firefox", Category="firefox")]
        [NUnit.Framework.TestCaseSourceAttribute("MultipleBrowserTags_chrome", Category="chrome")]
        public virtual void MultipleBrowserTags(string browser)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Multiple browser tags", new string[] {
                        "Browser:firefox",
                        "Browser:chrome",
                        "Duplicate:Property",
                        "Duplicate:Property"});
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 2 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("Repeat:3")]
        [NUnit.Framework.Property("Repeat", "3")]
        [NUnit.Framework.TestCaseSourceAttribute("Repeated3Times_browser", Category="browser")]
        public virtual void Repeated3Times(string browser)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Repeated 3 times", new string[] {
                        "Browser:browser",
                        "Repeat:3"});
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 1 testcase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("Repeat:3")]
        [NUnit.Framework.Property("Repeat", "3")]
        [NUnit.Framework.TestCaseSourceAttribute("Repeated3TimesWith2OutlineValues_browser", Category="browser")]
        public virtual void Repeated3TimesWith2OutlineValues(string browser, string header, string[] exampleTags)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "Browser:browser",
                    "Repeat:3"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Repeated 3 times with 2 outline values", @__tags);
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 1 testcase", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.IgnoreAttribute()]
        [NUnit.Framework.CategoryAttribute(":")]
        [NUnit.Framework.CategoryAttribute("a:")]
        [NUnit.Framework.CategoryAttribute(":a")]
        [NUnit.Framework.TestCaseAttribute("value", null)]
        [NUnit.Framework.TestCaseAttribute("other value", null)]
        public virtual void NoTagsScenarioOutline(string header, string[] exampleTags)
        {
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "ignore",
                    ":",
                    "a:",
                    ":a"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("No tags scenario outline", @__tags);
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should throw a no browser exception", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseSourceAttribute("ScenarioOutlineWithSingleBrowserTag_scenariooutlinebrowser", Category="scenario-outline-browser")]
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
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 1 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.TestCaseSourceAttribute("ScenarioOutlineWithTwoBrowserTags_chrome", Category="chrome")]
        [NUnit.Framework.TestCaseSourceAttribute("ScenarioOutlineWithTwoBrowserTags_firefox", Category="firefox")]
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
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 2 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.CategoryAttribute("OutlineTag")]
        [NUnit.Framework.CategoryAttribute("CustomProperty:PropertyValue")]
        [NUnit.Framework.Property("CustomProperty", "PropertyValue")]
        [NUnit.Framework.TestCaseSourceAttribute("ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples_chrome", Category="chrome")]
        [NUnit.Framework.TestCaseSourceAttribute("ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples_firefox", Category="firefox")]
        public virtual void ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples(string browser, string header, string[] exampleTags)
        {
            this.Browser = browser;
            GuardBrowserTagMissing();
            string[] @__tags = new string[] {
                    "Browser:chrome",
                    "Browser:firefox",
                    "OutlineTag",
                    "CustomProperty:PropertyValue"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("scenario outline with two browser tags and tagged examples", @__tags);
            this.ScenarioSetup(scenarioInfo);
            this.FeatureBackground();
            testRunner.Then("the test method should have 4 testcases", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
