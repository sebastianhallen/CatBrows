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
	    private const string REPEATS_KEY = "TestCaseRepeats";

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
            var guardBrowserTagMissing = new CodeMemberMethod {Name = GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME};
            guardBrowserTagMissing.Statements.AddRange(new CodeStatement[]
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
                    //bool shouldGuard = !(hasConfigSetting && !enforceExistenceOfBrowserTag)
                    new CodeVariableDeclarationStatement(typeof(bool), "shouldGuard",
                        new CodeBinaryOperatorExpression(
                            new CodeBinaryOperatorExpression(
                                new CodeVariableReferenceExpression("hasConfigSetting"), 
                                CodeBinaryOperatorType.BitwiseAnd, 
                                new CodeBinaryOperatorExpression(
                                    new CodeVariableReferenceExpression("enforceExistenceOfBrowserTag"),
                                    CodeBinaryOperatorType.IdentityInequality, 
                                    new CodePrimitiveExpression(true)
                                )
                           ), 
                           CodeBinaryOperatorType.IdentityInequality, 
                           new CodePrimitiveExpression(true)
                        )
                    ),
                    //if (shouldGuard && !hasBrowser) throw new System.Exception(browserMissingMessage);
                    new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeVariableReferenceExpression("shouldGuard"), 
                            CodeBinaryOperatorType.BitwiseAnd, 
                            new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("hasBrowser"), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(true))
                        ),
                        new CodeThrowExceptionStatement(new CodeObjectCreateExpression(typeof(Exception), new CodeVariableReferenceExpression("browserMissingMessage")))
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
            //this.Browser = null
            var setBrowserToNull = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Browser"), new CodePrimitiveExpression(null));
            generationContext.TestInitializeMethod.Statements.Insert(0, setBrowserToNull);
           
            //add browser to the scenario context
            //browser is set at the top of each test method for features with @Browser tags
            //CreateStatement(@"ScenarioContext.Current.Add(""Browser"", this.Browser)");
            var setBrowserOnContext = new CodeMethodInvokeExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeTypeReferenceExpression("TechTalk.SpecFlow.ScenarioContext"),
                                            "Current"
                                        ),
                                        "Add",
                                        new CodePrimitiveExpression("Browser"),
                                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Browser")
                                     );
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
            testMethod.Statements.Insert(0, new CodeExpressionStatement(
                                                new CodeMethodInvokeExpression(
                                                    new CodeMethodReferenceExpression(
                                                        new CodeThisReferenceExpression(), 
                                                        GUARD_BROWSER_TAG_PRESENCE_METHOD_NAME
                                                    )
                                                )
                                            ));
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

                //if present, get the @Repeat:<number of repeats> value and store it in UserData so we can use it in the scenario outline case later
                var repeats = GetRepeats(uniqueProperties);
                testMethod.UserData.Add(REPEATS_KEY, repeats + "");
                    
                foreach (var browser in browsers)
                {
                    //store browser in user data so we can use it later if this is a row test/scenario ouline
                    testMethod.UserData.Add(BROWSER_TAG_PREFIX + browser, browser);

                    //create a property that contains the test data
                    var testCaseSource = CreateTestCaseSource(generationContext, testMethod.Name, browser, repeats);
                    AddTestCaseSourceRow(generationContext, testCaseSource, new[] { browser });
                    
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
                var assignBrowserStatement = new CodeAssignStatement(
                    new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "Browser"), 
                    new CodeArgumentReferenceExpression("browser")
                );
                testMethod.Statements.Insert(0, assignBrowserStatement);
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
            var repeats = testMethod.UserData[REPEATS_KEY] as string;
            
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
				var testCaseSourceRows = new Dictionary<string, IEnumerable<string>>();

				//extract data for all test case source attributes that should be added
				foreach (var browser in browsers)
				{
					var testCaseSource = CreateTestCaseSource(generationContext, testMethod.Name + "_outline_", browser.ToString(), int.Parse(repeats), tags);
					var row = new[] { browser.ToString() }.Concat(arguments).Concat(new string[] { null });
					AddTestCaseSourceRow(generationContext, testCaseSource, row);

					var categories = tags.Concat(new[] { browser.ToString() });
					testCaseSourceRows[testCaseSource] = categories;
				}

				//... and add test case source attributes
				foreach (var testCaseSourceRow in testCaseSourceRows)
				{
					//check first that we haven't already added a matching test case source attribute in a previous call to SetRow(...)
					var hasExistingTestCaseAttribute = testMethod.CustomAttributes
														   .Cast<CodeAttributeDeclaration>()
														   .Where(attribute => attribute.Name.Equals("NUnit.Framework.TestCaseSourceAttribute"))
														   .Any(attribute =>
															   {
																   var sourceNameArg = (CodePrimitiveExpression)attribute.Arguments.Cast<CodeAttributeArgument>().First().Value;
																   return sourceNameArg.Value.ToString().Equals(testCaseSourceRow.Key);
															   });
					if (hasExistingTestCaseAttribute) continue;

					//ok, we don't hve this test case source attribute already, add it!

					var testCaseRowAttributeArgs = new[]
						{
							// first argument == test case source data
							new CodeAttributeArgument(new CodePrimitiveExpression(testCaseSourceRow.Key)),
							// add browser value as category
							new CodeAttributeArgument("Category", new CodePrimitiveExpression(string.Join(",", testCaseSourceRow.Value)))
						};
					
					CodeDomHelper.AddAttribute(testMethod, ROW_SOURCE_ATTR, testCaseRowAttributeArgs);
				}

                //since test case sources for non-row tests are added in a previous step we need to remove those
                //remove all test case source attributes that don't start with testMethod.Name + "_outline_"
                var superfluousAttributes = testMethod.CustomAttributes
                                                           .Cast<CodeAttributeDeclaration>()
                                                           .Where(attribute => attribute.Name.Equals("NUnit.Framework.TestCaseSourceAttribute"))
                                                           .Where(attribute =>
                                                               {
                                                                   var sourceNameArg = (CodePrimitiveExpression)attribute.Arguments.Cast<CodeAttributeArgument>().First().Value;
                                                                   return !sourceNameArg.Value.ToString().StartsWith(testMethod.Name + "_outline_");
                                                               })
                                                           .ToArray();
                foreach (var attribute in superfluousAttributes)
                {
                    testMethod.CustomAttributes.Remove(attribute);
                }

                //remove the properties with unused test case source data
                var superfluousProperties = generationContext.TestClass.Members.Cast<CodeTypeMember>()
                                                .Where(member => member is CodeMemberProperty).Cast<CodeMemberProperty>()
                                                .Where(property => property.Name.StartsWith(testMethod.Name))
                                                .Where(property => !property.Name.StartsWith(testMethod.Name + "_outline_"))
                                                .ToArray();
                foreach (var property in superfluousProperties)
                {
                    generationContext.TestClass.Members.Remove(property);
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

        private static int GetRepeats(Dictionary<string, string> uniqueProperties)
        {
            var repeatTag = "";
            int repeats = 1;
            if (uniqueProperties.TryGetValue("repeat", out repeatTag) || uniqueProperties.TryGetValue("Repeat", out repeatTag))
            {
                if (!int.TryParse(repeatTag, out repeats))
                {
                    repeats = 1;
                }
            }
            return repeats;
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
        private static string CreateTestCaseSource(TestClassGenerationContext generationContext, string testMethodName, string browser, int repeats, IEnumerable<string> tags = null)
        {

            var testCaseSourceName = ToMethodSafeString(new [] {testMethodName, browser}.Concat(tags ?? new string[] {}));
                
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


                //var repeats = 1
                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.int32", "repeats", new CodePrimitiveExpression(repeats)));
                //var rows = new List<TestCaseData>();
                property.GetStatements.Add(new CodeVariableDeclarationStatement("System.Collections.Generic.List<NUnit.Framework.TestCaseData>", "rows", new CodeObjectCreateExpression("System.Collections.Generic.List<NUnit.Framework.TestCaseData>")));
                
                //handle repeats by duplicating each row <repeats> number of times
                /*
                var repeatedRows = new List<TestCaseData>();
                var repeatedEnumerator = rows.GetEnumerator()
                while (repeatedEnumerator.MoveNext()) 
                {
                    var current = repeatedEnumerator.Current;
                    for (int i = 0; i < repeats; ++i) repeatedData.Add(data);
                    return repeatedData;
                }
                */

                //var repeatedRows = new List<TestCaseData>()
                property.GetStatements.Add(new CodeVariableDeclarationStatement(
                                                "System.Collections.Generic.List<NUnit.Framework.TestCaseData>",
                                                "repeatedRows",
                                                new CodeObjectCreateExpression("System.Collections.Generic.List<NUnit.Framework.TestCaseData>")
                                          )
                );
                //var repeatedEnumerator = filteredRows.GetEnumerator()
                property.GetStatements.Add(new CodeVariableDeclarationStatement(
                                                "System.Collections.Generic.List<NUnit.Framework.TestCaseData>.Enumerator",
                                                "repeatedEnumerator",
                                                new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("rows"), "GetEnumerator")
                                          )
                );

                //while (repeatedEnumerator.MoveNext()) 
                property.GetStatements.Add(new CodeIterationStatement(
                                                new CodeSnippetStatement(),
                                                new CodeMethodInvokeExpression(
                                                    new CodeVariableReferenceExpression("repeatedEnumerator"), "MoveNext"
                                                ),
                                                new CodeSnippetStatement(),
                                                    //while loop body
                                                    //var current = repeatedEnumerator.Current;
                                                    new CodeVariableDeclarationStatement(
                                                        "NUnit.Framework.TestCaseData",
                                                        "current",
                                                        new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("repeatedEnumerator"), "Current")
                                                    ),
                                                    //for (int i = 0; i < repeats; ++i)
                                                    new CodeIterationStatement(
                                                        new CodeVariableDeclarationStatement(typeof(int), "i", new CodePrimitiveExpression(0)),
                                                        new CodeBinaryOperatorExpression(
                                                            new CodeVariableReferenceExpression("i"), 
                                                            CodeBinaryOperatorType.LessThan, 
                                                            new CodeVariableReferenceExpression("repeats")
                                                        ),
                                                        new CodeAssignStatement(
                                                            new CodeVariableReferenceExpression("i"), 
                                                            new CodeBinaryOperatorExpression(
                                                                new CodeVariableReferenceExpression("i"),
                                                                CodeBinaryOperatorType.Add, 
                                                                new CodePrimitiveExpression(1)
                                                            )
                                                        ),
                                                        //inner for-loop body
                                                        //repeatedData.Add(data);
                                                        new CodeExpressionStatement(
                                                            new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("repeatedRows"), "Add", new CodeVariableReferenceExpression("current"))
                                                        )
                                                    )
                                           )
                );

                //return repeatedRows.ToArray()
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

            var newTestCaseDataExpression =
                    new CodeObjectCreateExpression(
                        new CodeTypeReference("NUnit.Framework.TestCaseData"),
                        row.Select(arg => new CodePrimitiveExpression(arg)).Cast<CodeExpression>().ToArray()
                    );

            //inject the new testdata entry after the declaration of repeats and rows.
            testCaseSourceProperty.GetStatements.Insert(2, new CodeExpressionStatement(
                new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("rows"), "Add", newTestCaseDataExpression)
            ));
        }

        private static string ToMethodSafeString(IEnumerable<string> parts)
        {
            return string.Join("__", parts.Select(ToMethodSafeString));
        }

        private static string ToMethodSafeString(string raw)
        {
            return new string(raw.Where(c => char.IsLetterOrDigit(c) || '_'.Equals(c)).ToArray());
        }
    }
}
