namespace CatBrows.Generator
{
	using System;
	using System.CodeDom;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Linq;
	using TechTalk.SpecFlow.Generator;
	using TechTalk.SpecFlow.Generator.UnitTestProvider;
	using TechTalk.SpecFlow.Utils;

	public class NUnitBrowserTestGenerator
        : IUnitTestGeneratorProvider
    {
        private const string PROPERTY_ATTR = "NUnit.Framework.Property";
        private const string TESTFIXTURE_ATTR = "NUnit.Framework.TestFixtureAttribute";
        private const string TEST_ATTR = "NUnit.Framework.TestAttribute";
        private const string ROW_ATTR = "NUnit.Framework.TestCaseAttribute";
        private const string ROW_SOURCE_ATTR = "NUnit.Framework.TestCaseSourceAttribute";
        private const string CATEGORY_ATTR = "NUnit.Framework.CategoryAttribute";
        private const string TESTSETUP_ATTR = "NUnit.Framework.SetUpAttribute";
        private const string TESTFIXTURESETUP_ATTR = "NUnit.Framework.TestFixtureSetUpAttribute";
        private const string TESTFIXTURETEARDOWN_ATTR = "NUnit.Framework.TestFixtureTearDownAttribute";
        private const string TESTTEARDOWN_ATTR = "NUnit.Framework.TearDownAttribute";
        private const string IGNORE_ATTR = "NUnit.Framework.IgnoreAttribute";
        private const string DESCRIPTION_ATTR = "NUnit.Framework.DescriptionAttribute";

        private const string ENFORCE_BROWSER_SETTING_KEY = "CatBrows-RequiresBrowser";
	    private const string NO_BROWSER_DEFINED_MESSSAGE_SETTINGS_KEY = "CatBrows-BrowserMissingMessage";
        private const string DEFAULT_NO_BROWSER_DEFINED_MESSAGE = "No browser defined, please specify @Browser:someBrowser for your scenario.";
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
            generationContext.Namespace.Imports.Add(new CodeNamespaceImport("System.Linq"));

            CodeDomHelper.AddAttribute(generationContext.TestClass, TESTFIXTURE_ATTR);
            CodeDomHelper.AddAttribute(generationContext.TestClass, DESCRIPTION_ATTR, featureTitle);
            CodeDomHelper.AddAttribute(generationContext.TestClass, CATEGORY_ATTR, generationContext.TestClass.Name);
            CodeDomHelper.AddAttribute(generationContext.TestClass, CATEGORY_ATTR, generationContext.Namespace.Name.Split('.').Last());


            //add a private string field, Browser, to the generated test class that we will use to set ScenarioContext.Current["Browser"] in each test
            generationContext.TestClass.Members.Add(new CodeMemberField(typeof (string), "Browser"));

            //create the browser guard method that enforces the use of @Browser tags for scenarios unless 
            //  <appSettings><add key="CatBrows-RequiresBrowser" value="false" /></appSettings>
            // is present in app.config
            var guardBrowserTagMissing = CreateMethod(GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME, new[]
                {
                    //var appSettings = ConfigurationManager.AppSettings;
                    new CodeVariableDeclarationStatement(typeof(NameValueCollection), "appSettings", 
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression("System.Configuration.ConfigurationManager"), "AppSettings")
                    ),
                    //var browserMissingMessage = "<DEFAULT_NO_BROWSER_DEFINED_MESSAGE>";
                    new CodeVariableDeclarationStatement(typeof(string), "browserMissingMessage", new CodePrimitiveExpression(DEFAULT_NO_BROWSER_DEFINED_MESSAGE)),
                    //var customBrowserMissingMessage = appSettings.Get("<NO_BROWSER_DEFINED_MESSSAGE_SETTINGS_KEY>");
                    new CodeVariableDeclarationStatement(typeof(string), "customBrowserMissingMessage", new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("appSettings"), "Get", new CodePrimitiveExpression(NO_BROWSER_DEFINED_MESSSAGE_SETTINGS_KEY))),
                    //if (customBrowserMissingMessage != null) browserMissingMessage = customBrowserMissingMessage
                    new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeVariableReferenceExpression("customBrowserMissingMessage"),
                            CodeBinaryOperatorType.IdentityInequality, 
                            new CodePrimitiveExpression(null)
                        ), 
                        new CodeAssignStatement(new CodeVariableReferenceExpression("browserMissingMessage"), new CodeVariableReferenceExpression("customBrowserMissingMessage"))
                    ),
                    //var enforceExistenceOfBrowserTagRaw = appSettings.Get("<ENFORCE_BROWSER_SETTING_KEY>");
                    new CodeVariableDeclarationStatement(typeof(string), "enforceExistenceOfBrowserTagRaw", 
                        new CodeMethodInvokeExpression(
                            new CodeVariableReferenceExpression("appSettings"), "Get", new CodePrimitiveExpression(ENFORCE_BROWSER_SETTING_KEY)
                        )
                    ),
                    //bool enforceExistenceOfBrowserTag;
                    new CodeVariableDeclarationStatement(typeof(bool), "enforceExistenceOfBrowserTag"), 
                    //bool hasConfigSetting = bool.TryParse(enforceExistenceOfBrowserTagRaw, out enforceExistenceOfBrowserTag)"
                    new CodeVariableDeclarationStatement(typeof(bool), "hasConfigSetting",
                        new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(bool)), "TryParse", 
                            new CodeVariableReferenceExpression("enforceExistenceOfBrowserTagRaw"), 
                            new CodeDirectionExpression(FieldDirection.Out, new CodeVariableReferenceExpression("enforceExistenceOfBrowserTag"))
                        )
                    ), 
                    //bool hasBrowser = !string.IsNullOrEmpty(this.Browser)"
                    new CodeVariableDeclarationStatement(typeof(bool), "hasBrowser", new CodeBinaryOperatorExpression(
                            new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(string)), "IsNullOrEmpty", new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Browser")),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePrimitiveExpression(true)
                        )),
                 


                    CreateStatement(@"bool shouldGuard = !(hasConfigSetting && !enforceExistenceOfBrowserTag)"),
                    new CodeConditionStatement(new CodeSnippetExpression("shouldGuard"),
                        new CodeConditionStatement(new CodeSnippetExpression("!hasBrowser"),
                                CreateThrowStatement(new CodeVariableReferenceExpression("browserMissingMessage"))
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
            var categoriesForAllTests = categories.Where(category => !category.StartsWith(BROWSER_TAG_PREFIX)).ToArray();
            CodeDomHelper.AddAttributeForEachValue(testMethod, CATEGORY_ATTR, categoriesForAllTests);

            //add Property attributes for all tags containing a ':' separated key value pair (except for browser)
            var properties = categoriesForAllTests
                .Where(category => category.Contains(":"))
                .Select(category => category.Split(':'))
                .Where(property => property.Count() == 2)
                .Select(property => new
                    {
                        Key = property[0].Trim(),
                        Value = property[1].Trim()
                    })
                .Where(property => !string.IsNullOrEmpty(property.Key) && !string.IsNullOrEmpty(property.Value));

            //filter out duplicate property keys
            var uniqueProperties = properties
                .GroupBy(property => property.Key)
                .Where(group => group.Count() == 1)
                .Select(group => group.First())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var property in uniqueProperties)
            {
                CodeDomHelper.AddAttribute(testMethod, PROPERTY_ATTR, property.Key, property.Value);
            }
            

            //and finally determine which browsers we should inject
            var browsers = categories
                .Where(category => category.StartsWith(BROWSER_TAG_PREFIX))
                .Select(category => category.Replace(BROWSER_TAG_PREFIX, ""))
                .ToArray();
            
            if (browsers.Any())
            {
                //inject an argument, string browser, as the first argument in the test method
                testMethod.Parameters.Insert(0, new CodeParameterDeclarationExpression("System.string", "browser"));

                foreach (var browser in browsers)
                {
                    //store browser in user data so we can use it later if this is a row test/scenario ouline
                    testMethod.UserData.Add(BROWSER_TAG_PREFIX + browser, browser);
                    
                    //create a property that contains the test data
                    var testCaseSource = CreateTestCaseSource(generationContext, testMethod.Name, browser);
                    AddTestCaseSourceRow(generationContext, testCaseSource, new[] { browser });
                    
                    //if present - update the repeat var in the test data
                    var repeat = "";
                    if (uniqueProperties.TryGetValue("repeat", out repeat) || uniqueProperties.TryGetValue("Repeat", out repeat))
                    {
                        int repeats;
                        if (int.TryParse(repeat, out repeats))
                        {
                            var tcs = generationContext.TestClass.Members.OfType<CodeMemberProperty>().First(member => member.Name.Equals(testCaseSource));
                            var repeatDeclarationStatement = (CodeVariableDeclarationStatement)tcs.GetStatements.Cast<CodeStatement>().First();
                            repeatDeclarationStatement.InitExpression = new CodePrimitiveExpression(repeats);
                        }
                    }


                    var browserArgument = new[]
                        {
                            // first argument == test case source data
                            new CodeAttributeArgument(new CodePrimitiveExpression(testCaseSource)),
                            // add browser value as category
                            new CodeAttributeArgument("Category", new CodePrimitiveExpression(browser))
                        };
                    this.CodeDomHelper.AddAttribute(testMethod, ROW_SOURCE_ATTR, browserArgument);

                }

                //Yay, we have a browser tag, assign it to the Browser field in the method body
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
                foreach (var browser in browsers)
                {
                    //var methodNameSafeTags = tags.Select(tag => new string(tag.Select(c => c).Where(char.IsLetterOrDigit).ToArray()));
                    //var tagSuffix = string.Join("_", methodNameSafeTags);
                    
                    //var testCaseSourceName = testMethod.Name;
                    
                    //var rawTestCaseSource = string.IsNullOrEmpty(tagSuffix)
                    //                             ? testCaseSourceName
                    //                             : testCaseSourceName + "_" + tagSuffix;

                    var testCaseSource = CreateTestCaseSource(generationContext, testMethod.Name, browser.ToString());
                    var row = new[] { browser.ToString() }.Concat(arguments).Concat(new string[] {null});
                    AddTestCaseSourceRow(generationContext, testCaseSource, row);
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

        /// <summary>
        /// checks if the generated test class has a method or property with the given name
        /// </summary>
        /// <param name="generationContext"></param>
        /// <param name="testMethodName"></param>
        /// <returns></returns>
	    private static bool HasExistingMember(TestClassGenerationContext generationContext, string testMethodName)
	    {
	        return generationContext.TestClass.Members.Cast<CodeTypeMember>().Any(member => testMethodName.Equals(member.Name));
	    }

        /// <summary>
        /// Creates a property with a list of NUnit.Framework.TestCaseData
        /// </summary>
        /// <param name="generationContext"></param>
        /// <param name="testMethodName"></param>
        /// <param name="browser"></param>
        /// <returns></returns>
	    private static string CreateTestCaseSource(TestClassGenerationContext generationContext, string testMethodName, string browser)
        {
            var testCaseSourceName = testMethodName + "_" + new string(browser.Where(char.IsLetterOrDigit).ToArray());
	        if (!HasExistingMember(generationContext, testCaseSourceName))
	        {
                var type = new CodeTypeReference(typeof(object[]));
                var property = new CodeMemberProperty
                    {
                        Type = type,
                        HasGet = true,
                        HasSet = false,
                        Name = testCaseSourceName
                    };

	            property.Attributes &= ~MemberAttributes.AccessMask & ~MemberAttributes.ScopeMask;
                property.Attributes |= MemberAttributes.FamilyAndAssembly | MemberAttributes.Static;


                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.int32", "repeats", new CodePrimitiveExpression(1)));
                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.Collections.Generic.List<NUnit.Framework.TestCaseData>", "rows", new CodeObjectCreateExpression("System.Collections.Generic.List<NUnit.Framework.TestCaseData>")));
                
                //since TestCaseData will be added both in SetRow and SetTest in the case of this being a scenario outline, we filter the test data to only return those added by scenario outlines if applicable.
                property.GetStatements.Add(new CodeVariableDeclarationStatement(typeof(int), "maxArguments",
                    new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("rows"), "Max", new CodeSnippetExpression("row => row.Arguments.Count()")))
                );
                
                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData>", "filteredRows",
                        new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("rows"), "Where", new CodeSnippetExpression("row => row.Arguments.Count().Equals(maxArguments)"))
                    )
                );

                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.Collections.Generic.IEnumerable<NUnit.Framework.TestCaseData>", "repeatedRows",
                        new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("filteredRows"), "SelectMany", new CodeSnippetExpression(@"data =>
                            {
                                var repeatedData = new System.Collections.Generic.List<NUnit.Framework.TestCaseData>();
                                for (int i = 0; i < repeats; ++i)
                                {
                                    repeatedData.Add(data);
                                }
                                return repeatedData;
                            }"))
                    )
                );


                property.GetStatements.Add(new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("repeatedRows"), "ToArray")
                    )
                );
                generationContext.TestClass.Members.Add(property);
            }
           
            return testCaseSourceName;
        }

        /// <summary>
        /// injects a line in the test case source property that adds a new TestCaseData entry into the rows list
        /// </summary>
        /// <param name="generationContext"></param>
        /// <param name="testCaseSource"></param>
        /// <param name="row"></param>
        private void AddTestCaseSourceRow(TestClassGenerationContext generationContext, string testCaseSource, IEnumerable<string> row)
        {
            var testCaseSourceProperty = (CodeMemberProperty)generationContext.TestClass.Members.Cast<CodeTypeMember>().Single(member => member.Name.Equals(testCaseSource));

            var arguments = string.Join(", ", row.Select(arg => arg == null ? "null" : string.Format(@"""{0}""", arg)));
            var testCaseData = new CodeSnippetExpression(string.Format("new NUnit.Framework.TestCaseData({0})", arguments));

            //inject the new testdata entry after the declaration of repeats and rows.
            testCaseSourceProperty.GetStatements.Insert(2, new CodeExpressionStatement(
                new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("rows"), "Add", testCaseData)
            ));
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
            return CreateThrowStatement(new CodePrimitiveExpression(message));
        }

        private static CodeStatement CreateThrowStatement(CodeExpression message)
        {
            return new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(Exception), message));
        }
    }
}
