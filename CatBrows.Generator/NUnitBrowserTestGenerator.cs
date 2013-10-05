namespace CatBrows.Generator
{
	using System;
	using System.CodeDom;
	using System.Collections.Generic;
	using System.Linq;
	using TechTalk.SpecFlow.Generator;
	using TechTalk.SpecFlow.Generator.UnitTestProvider;
	using TechTalk.SpecFlow.Utils;

	public class NUnitBrowserTestGenerator
        : IUnitTestGeneratorProvider
    {
        private const string TESTFIXTURE_ATTR = "NUnit.Framework.TestFixtureAttribute";
        private const string TEST_ATTR = "NUnit.Framework.TestAttribute";
        private const string ROW_ATTR = "NUnit.Framework.TestCaseAttribute";
        private const string CATEGORY_ATTR = "NUnit.Framework.CategoryAttribute";
        private const string TESTSETUP_ATTR = "NUnit.Framework.SetUpAttribute";
        private const string TESTFIXTURESETUP_ATTR = "NUnit.Framework.TestFixtureSetUpAttribute";
        private const string TESTFIXTURETEARDOWN_ATTR = "NUnit.Framework.TestFixtureTearDownAttribute";
        private const string TESTTEARDOWN_ATTR = "NUnit.Framework.TearDownAttribute";
        private const string IGNORE_ATTR = "NUnit.Framework.IgnoreAttribute";
        private const string DESCRIPTION_ATTR = "NUnit.Framework.DescriptionAttribute";

	    private const string ENFORCE_BROWSER_SETTING_KEY = "CatBrows-RequiresBrowser";
        private const string NO_BROWSER_DEFINED = "No browser defined, please specify @Browser:someBrowser for your scenario.";
        private const string GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME = "GuardBrowserTagMissing";
        private const string BROWSER_TAG_PREFIX = "Browser:";

        protected CodeDomHelper CodeDomHelper { get; set; }

        public bool SupportsRowTests { get { return true; } }
        public bool SupportsAsyncTests { get { return false; } }

        public NUnitBrowserTestGenerator(CodeDomHelper codeDomHelper)
        {
            this.CodeDomHelper = codeDomHelper;
        }

        public void SetTestClass(TestClassGenerationContext generationContext, string featureTitle, string featureDescription)
        {
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Configuration"));

            CodeDomHelper.AddAttribute(generationContext.TestClass, TESTFIXTURE_ATTR);
            CodeDomHelper.AddAttribute(generationContext.TestClass, DESCRIPTION_ATTR, featureTitle);

            //add a private string field, Browser, to the generated test class that we will use to set ScenarioContext.Current["Browser"] in each test
            generationContext.TestClass.Members.Add(new CodeMemberField(typeof (string), "Browser"));

            //create the browser guard method that enforces the use of @Browser tags for scenarios unless 
            //  <appSettings><add key="CatBrows-RequiresBrowser" value="false" /></appSettings>
            // is present in app.config
            var guardBrowserTagMissing = CreateMethod(GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME, new[]
                {
                    CreateStatement(@"var enforceExistenceOfBrowserTagRaw = ConfigurationManager.AppSettings[""" + ENFORCE_BROWSER_SETTING_KEY + @"""]"),
                    CreateStatement(@"bool enforceExistenceOfBrowserTag"),
                    CreateStatement(@"bool hasConfigSetting = bool.TryParse(enforceExistenceOfBrowserTagRaw, out enforceExistenceOfBrowserTag)"),
                    CreateStatement(@"bool hasBrowser = !string.IsNullOrEmpty(this.Browser)"),
                    CreateStatement(@"bool shouldGuard = !(hasConfigSetting && !enforceExistenceOfBrowserTag)"),
                    new CodeConditionStatement(new CodeSnippetExpression("shouldGuard"),
                        new CodeConditionStatement(new CodeSnippetExpression("!hasBrowser"),
                                CreateThrowStatement(NO_BROWSER_DEFINED)
                            )
                    )
                });

            generationContext.TestClass.Members.Add(guardBrowserTagMissing);
        }



        public void SetTestClassCategories(TestClassGenerationContext generationContext, IEnumerable<string> featureCategories)
        {
            CodeDomHelper.AddAttributeForEachValue(generationContext.TestClass, CATEGORY_ATTR, featureCategories);
        }

        public void SetTestClassIgnore(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestClass, IGNORE_ATTR);
        }

        public virtual void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            // by default, doing nothing to the final generated code
        }


        public void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestClassInitializeMethod, TESTFIXTURESETUP_ATTR);
        }

        public void SetTestClassCleanupMethod(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestClassCleanupMethod, TESTFIXTURETEARDOWN_ATTR);
        }


        public void SetTestInitializeMethod(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestInitializeMethod, TESTSETUP_ATTR);
            generationContext.TestInitializeMethod.Statements.Insert(0, CreateStatement("this.Browser = null"));
           
            //add browser to the scenario context
            //browser is set at the top of each test method for features with @Browser tags
            var setBrowserOnContext = CreateStatement(@"ScenarioContext.Current.Add(""Browser"", this.Browser)");
            generationContext.ScenarioInitializeMethod.Statements.Add(setBrowserOnContext);
        }

        public void SetTestCleanupMethod(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestCleanupMethod, TESTTEARDOWN_ATTR);
        }


        public void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            CodeDomHelper.AddAttribute(testMethod, TEST_ATTR);

            //store the description for later
            testMethod.UserData.Add(DESCRIPTION_ATTR, scenarioTitle);

            //inject the guard statement
            testMethod.Statements.Insert(0, CreateStatement(GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME + "()"));
        }

        public void SetTestMethodCategories(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> scenarioCategories)
        {
            //we don't want "@Browser:browser" as separate categories. We only want these categories on the actual test case rows.
            var categories = scenarioCategories.ToArray();
            var categoriesForAllTests = categories.Where(category => !category.StartsWith(BROWSER_TAG_PREFIX));
            CodeDomHelper.AddAttributeForEachValue(testMethod, CATEGORY_ATTR, categoriesForAllTests);

            var browsers = categories
                .Where(category => category.StartsWith(BROWSER_TAG_PREFIX))
                .Select(category => category.Replace(BROWSER_TAG_PREFIX, ""))
                .ToArray();
            
            
            if (browsers.Any())
            {
                //inject an argument, string browser, as the first argument in the test method
                testMethod.Parameters.Insert(0, new CodeParameterDeclarationExpression("System.string", "browser"));

                var description = (string)testMethod.UserData[DESCRIPTION_ATTR];
                foreach (var browser in browsers)
                {
                    //store browser in user data so we can use it later if this is a row test/scenario ouline
                    testMethod.UserData.Add(BROWSER_TAG_PREFIX + browser, browser);
                    
                    //add TestCase-Attributes ...
                    var browserArgument = new[]
                        {
                            // first argument == the browser value
                            new CodeAttributeArgument(new CodePrimitiveExpression(browser)),
                            // description
                            new CodeAttributeArgument("Description", new CodePrimitiveExpression(description + " (" + browser + ")")),
                            // add browser value as category
                            new CodeAttributeArgument("Category", new CodePrimitiveExpression(browser))
                        };
                    this.CodeDomHelper.AddAttribute(testMethod, ROW_ATTR, browserArgument);
                }

                //Yay, we have a browser tag
                testMethod.Statements.Insert(0, CreateStatement("this.Browser = browser"));
            }
        }

        public void SetTestMethodIgnore(TestClassGenerationContext generationContext, CodeMemberMethod testMethod)
        {
            CodeDomHelper.AddAttribute(testMethod, IGNORE_ATTR);
        }


        public void SetRowTest(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            SetTestMethod(generationContext, testMethod, scenarioTitle);
        }

        public void SetRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> arguments, IEnumerable<string> tags, bool isIgnored)
        {
            //we extracted the browser and added it to testMethod.UserData so we could have access to it here.
            var browsers = testMethod.UserData
                                     .Keys.OfType<string>()
                                     .Where(key => key.StartsWith(BROWSER_TAG_PREFIX))
                                     .Select(key => testMethod.UserData[key])
                                     .ToArray();

            #region specflows NUnitTestGeneratorProvider.cs
            var args = arguments.Select(
              arg => new CodeAttributeArgument(new CodePrimitiveExpression(arg))).ToList();

            // addressing ReSharper bug: TestCase attribute with empty string[] param causes inconclusive result - https://github.com/techtalk/SpecFlow/issues/116
            var exampleTagExpressionList = tags.Select(t => new CodePrimitiveExpression(t)).ToArray();
            CodeExpression exampleTagsExpression = exampleTagExpressionList.Length == 0 ?
                (CodeExpression)new CodePrimitiveExpression(null) :
                new CodeArrayCreateExpression(typeof(string[]), exampleTagExpressionList);
            args.Add(new CodeAttributeArgument(exampleTagsExpression));

            if (isIgnored)
                args.Add(new CodeAttributeArgument("Ignored", new CodePrimitiveExpression(true)));
 #endregion

            if (browsers.Any())
            {
                //remove TestCaseAttributes added from SetTestMethod
                // TestCaseAttributes from SetTestMethod will only have 
                //three arguments (browser + Description + category) when the "real" will have at least four:
                //browser, arguments-from-example..., exampleTags
                var superfluousAttributes = testMethod.CustomAttributes
                                                      .Cast<CodeAttributeDeclaration>()
                                                      .Where(attribute => attribute.Arguments.Count == 3)
                                                      .ToArray();
                foreach (var attribute in superfluousAttributes)
                {
                    testMethod.CustomAttributes.Remove(attribute);
                }

                //add a testcase row for each occurrence of @Browser-tag
                var description = (string)testMethod.UserData[DESCRIPTION_ATTR];
                foreach (var browser in browsers)
                {
                    var categories = tags.Union(new[] { browser });
                    var rowArguments = new[] { new CodeAttributeArgument(new CodePrimitiveExpression(browser)) }
                        .Concat(args)
                        .Concat(new[]
                        {
                            new CodeAttributeArgument("Description", new CodePrimitiveExpression(description + " (" + browser + ")")),
                            new CodeAttributeArgument("Category", new CodePrimitiveExpression(string.Join(",", categories)))
                        })
                        .ToArray();
                    CodeDomHelper.AddAttribute(testMethod, ROW_ATTR, rowArguments);
                }
            }
            else
            {
                //no browser tag present, add the default test case attribute and let the browser tag guard handle this at run time
                CodeDomHelper.AddAttribute(testMethod, ROW_ATTR, args.ToArray());
            }
        }

        public void SetTestMethodAsRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle, string exampleSetName, string variantName, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            // doing nothing since we support RowTest
        }

        private static CodeMemberMethod CreateMethod(string name, IEnumerable<CodeStatement> statements)
        {
            var method = new CodeMemberMethod { Name = name };
            foreach (var statement in statements)
            {
                method.Statements.Add(statement);
            }
            return method;
        }

        private static CodeStatement CreateStatement(string statement)
        {
            return new CodeSnippetStatement(@"            " + statement + ";");
        }

        private static CodeStatement CreateThrowStatement(string message)
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(Exception), new CodePrimitiveExpression(message)));
        }
    }
}
