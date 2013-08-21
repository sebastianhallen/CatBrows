namespace Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using CatBrows.Generator;
    using NUnit.Framework;
    using TechTalk.SpecFlow;
    
    [TestFixture]
    public class IntegrationTests
    {
        private OverloadedBrowserTestFeature generatedTestCase;

        [SetUp]
        public void Before()
        {
            this.generatedTestCase = new OverloadedBrowserTestFeature();
        }

        [TearDown]
        public void After()
        {
            this.generatedTestCase.FeatureTearDown();
        }

        [Test]
        public void Should_tag_class_with_testfixture_attribute()
        {
            var fixtureAttribute = Attribute.GetCustomAttribute(typeof(BrowserTestFeature), typeof(TestFixtureAttribute));

            Assert.That(fixtureAttribute, Is.Not.Null);
        }

        [Test]
        public void Should_throw_exception_when_no_tags_at_all_are_supplied()
        {
			this.VerifyNoBrowserDefinedException(() => this.generatedTestCase.NoTagsAtAll());
        }

        [Test]
        public void Should_throw_exception_when_no_browser_tag_is_supplied_with_other_tags()
        {
			this.VerifyNoBrowserDefinedException(() => this.generatedTestCase.TagsButNoBrowserTag());
        }

        [Test]
        public void Should_throw_no_browser_defined_exception_for_scenario_outlines_without_tags()
        {
			this.VerifyNoBrowserDefinedException(() => this.generatedTestCase.NoTagsScenarioOutline("some value", null));
        }

        [Test]
        public void Should_throw_no_browser_defined_exception_for_scenario_outlines_with_tags_but_without_browser_tags()
        {
			this.VerifyNoBrowserDefinedException(() => this.generatedTestCase.TagsButNoBrowserTagScenarioOutline("some value", null));
        }


        [Test]
        public void Should_add_each_browser_as_category_to_scenario_methods()
        {
            var categories = this.GetMethodAttributes<CategoryAttribute>(() => this.generatedTestCase.MultipleBrowserTags("browser"));

            var chrome = categories.SingleOrDefault(category => category.Name.Equals("chrome"));
            var firefox = categories.SingleOrDefault(category => category.Name.Equals("firefox"));

            Assert.That(chrome, Is.Not.Null, "Chrome was not set as category");
            Assert.That(firefox, Is.Not.Null, "Firefox was not set as category");
        }

        [Test]
        public void Should_decorate_method_with_test_case_attribute_when_supplying_a_browser_tag_to_a_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.SingleBrowserTagChrome("browser"));

            var tca = testCaseAttributes.SingleOrDefault();
            Assert.That(tca, Is.Not.Null);
            Assert.That(tca.Arguments.Single(), Is.EqualTo("chrome"));
        }

        [Test]
        public void Should_decorate_method_with_test_case_attributes_for_each_browser_tag_when_supplying_multiple_browser_tags_to_a_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.MultipleBrowserTags("browser"));

            var chrome = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "chrome"));
            var firefox = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "firefox"));

            Assert.That(chrome, Is.Not.Null);
            Assert.That(firefox, Is.Not.Null);
        }

        [Test]
        public void Should_initialize_browser_when_running_browser_tagged_scenario()
        {
            this.generatedTestCase.SingleBrowserTagChrome("some browser");

            Assert.That(ScenarioContext.Current["Browser"], Is.EqualTo("some browser"));
        }

        [Test]
        public void Browser_should_be_set_when_background_is_invoked()
        {
            this.generatedTestCase.SingleBrowserTagChrome("some browser");

            Assert.That(BrowserTestSteps.BackgroundHasBrowser);
        }

        [Test]
        public void Should_add_one_test_case_per_browser_for_each_row_in_a_scenario_ouline_example()
        {
            var categories = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.ScenarioOutlineWithTwoBrowserTags("browser", "", null));

            Assert.That(categories.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Row_test_attributes_should_have_browser_as_first_argument()
        {
            var categories = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.ScenarioOutlineWithTwoBrowserTags("foo", "bar", null));

            var chromeRow = categories.SingleOrDefault(a => a.Arguments.First().Equals("chrome"));
            var firefoxRow = categories.SingleOrDefault(a => a.Arguments.First().Equals("firefox"));

            Assert.That(chromeRow, Is.Not.Null);
            Assert.That(firefoxRow, Is.Not.Null);
        }

        [Test]
        public void Should_set_browser_tagged_description_to_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.SingleBrowserTagChrome("browser"));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "Single browser tag chrome" + " (chrome)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }

        [Test]
        public void Should_set_browser_tagged_description_to_scenario_outline_testcase()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.ScenarioOutlineWithSingleBrowserTag("browser", "", null));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "scenario outline with single browser tag (scenario-outline-browser)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }

		private void VerifyNoBrowserDefinedException(Action action)
		{
			var exception = Assert.Throws<Exception>(() => action());

			Assert.That(exception.Message, Is.EqualTo("No browser defined, please specify @Browser:someBrowser for your scenario."));
		}


        private TAttribute[] GetMethodAttributes<TAttribute>(Expression<Action> expression)
            where TAttribute : Attribute
        {
            var methodBody = (MethodCallExpression)expression.Body;
            var methodInfo = methodBody.Method;

            var attributes = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Cast<TAttribute>().ToArray();
        }
    }
}
