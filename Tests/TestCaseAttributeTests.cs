namespace CatBrows.Generator.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using TestSample;
    using TestSample.DefaultSettings;

    [TestFixture]
    public class TestCaseAttributeTests
        : GenerationTest<BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_create_test_case_source_with_browser_suffix_for_each_browser()
        {
            var chromeArguments = (object[])BrowserRequiredTestFeature.MultipleBrowserTags_chrome.Single();
            var firefoxArguments = (object[])BrowserRequiredTestFeature.MultipleBrowserTags_firefox.Single();
            
            Assert.That(chromeArguments.Single(), Is.EqualTo("chrome"));
            Assert.That(firefoxArguments.Single(), Is.EqualTo("firefox"));
        }


        /* deprecated */
         
        [Test]
        public void Should_decorate_method_with_test_case_attribute_when_supplying_a_browser_tag_to_a_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.SingleBrowserTagChrome("browser"));

            var tca = testCaseAttributes.SingleOrDefault();
            Assert.That(tca, Is.Not.Null);
            Assert.That(tca.Arguments.Single(), Is.EqualTo("chrome"));
        }

        [Test]
        public void Should_decorate_method_with_test_case_attributes_for_each_browser_tag_when_supplying_multiple_browser_tags_to_a_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.MultipleBrowserTags("browser"));

            var chrome = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "chrome"));
            var firefox = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "firefox"));

            Assert.That(chrome, Is.Not.Null);
            Assert.That(firefox, Is.Not.Null);
        }

        [Test]
        public void Should_add_one_test_case_per_browser_for_each_row_in_a_scenario_ouline_example()
        {
            var rows = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.ScenarioOutlineWithTwoBrowserTags("browser", "", null));

            Assert.That(rows.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Row_test_attributes_should_have_browser_as_first_argument()
        {
            var categories = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.ScenarioOutlineWithTwoBrowserTags("foo", "bar", null));

            var chromeRow = categories.SingleOrDefault(a => a.Arguments.First().Equals("chrome"));
            var firefoxRow = categories.SingleOrDefault(a => a.Arguments.First().Equals("firefox"));

            Assert.That(chromeRow, Is.Not.Null);
            Assert.That(firefoxRow, Is.Not.Null);
        }

        [Test]
        public void Should_not_remove_test_case_rows_when_browser_is_not_set_for_scenario_outlines()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.TagsButNoBrowserTagScenarioOutline("some value", null));

            Assert.That(testCaseAttributes.Any());
        }

        [Test]
        public void Should_set_browser_tagged_description_to_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.SingleBrowserTagChrome("browser"));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "Single browser tag chrome" + " (chrome)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }

        [Test]
        public void Should_set_browser_tagged_description_to_scenario_outline_testcase()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.ScenarioOutlineWithSingleBrowserTag("browser", "", null));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "scenario outline with single browser tag (scenario-outline-browser)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }
    }
}