namespace BrowserTestGenerator
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
            CodeDomHelper.AddAttribute(generationContext.TestClass, TESTFIXTURE_ATTR);
            CodeDomHelper.AddAttribute(generationContext.TestClass, DESCRIPTION_ATTR, featureTitle);
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
        }

        public void SetTestCleanupMethod(TestClassGenerationContext generationContext)
        {
            CodeDomHelper.AddAttribute(generationContext.TestCleanupMethod, TESTTEARDOWN_ATTR);
        }


        public void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
            CodeDomHelper.AddAttribute(testMethod, TEST_ATTR);
            CodeDomHelper.AddAttribute(testMethod, DESCRIPTION_ATTR, scenarioTitle);

            //add a throw statement as the first line, this will be removed by SetTestMethodCategories but is needed in the cases where no tags at all are supplied
            testMethod.Statements.Insert(0, CreateThrowStatement<NoBrowserDefinedException>());
        }

        public void SetTestMethodCategories(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, IEnumerable<string> scenarioCategories)
        {
            //we don't want the "Browser:"-part of the browser categories
            var categories = scenarioCategories.Select(category => category.Replace(BROWSER_TAG_PREFIX, ""));
            CodeDomHelper.AddAttributeForEachValue(testMethod, CATEGORY_ATTR, categories);

            var browsers = scenarioCategories
                .Where(category => category.StartsWith(BROWSER_TAG_PREFIX))
                .Select(category => category.Replace(BROWSER_TAG_PREFIX, ""))
                .ToArray();
            
            
            if (browsers.Any())
            {
                //Augument the test method with a browser argument
                testMethod.Parameters.Insert(0, new CodeParameterDeclarationExpression("System.string", "browser"));

                //add TestCase-Attributes with first argument == the browser value
                foreach (var browser in browsers)
                {
                    var browserArgument = new[] { new CodeAttributeArgument(new CodePrimitiveExpression(browser)) };
                    this.CodeDomHelper.AddAttribute(testMethod, ROW_ATTR, browserArgument);
                }

                //start by removing the throw statement added by SetTestMethod
                testMethod.Statements.RemoveAt(0);
                //... and replace it with a browser initialization
                var browserInitializationStatment = new CodeExpressionStatement(new CodeSnippetExpression(@"ScenarioContext.Current.Add(""Browser"", browser);"));
                testMethod.Statements.Insert(0, browserInitializationStatment);
                        
            }
            //when no browser tag is present, replace method body with a throw new NoBrowserDefinedException
            else
            {
                this.ReplaceMethodBody(testMethod, CreateThrowStatement<NoBrowserDefinedException>());
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

            CodeDomHelper.AddAttribute(testMethod, ROW_ATTR, args.ToArray());
        }

        public void SetTestMethodAsRow(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle, string exampleSetName, string variantName, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            // doing nothing since we support RowTest
        }

        private void ReplaceMethodBody(CodeMemberMethod testMethod, CodeStatement newBody)
        {
            var numberOfRows = testMethod.Statements.Count;
            for (var i = numberOfRows; i > 0; --i)
            {
                testMethod.Statements.RemoveAt(0);
            }

            testMethod.Statements.Add(newBody);
        }
        
        private static CodeStatement CreateThrowStatement<TException>()
            where TException : Exception
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(TException)));
        }
    }
}
