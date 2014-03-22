namespace CatBrows.Generator.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using TestSample;
    using TestSample.DefaultSettings;

    [TestFixture]
    public class TestDataSourceTests
        : GenerationTest<BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_create_test_case_source_with_browser_suffix_for_each_browser_in_a_scenario()
        {
            var chromeArguments = (TestCaseData)BrowserRequiredTestFeature.MultipleBrowserTags__chrome.Single();
            var firefoxArguments = (TestCaseData)BrowserRequiredTestFeature.MultipleBrowserTags__firefox.Single();

            Assert.That(chromeArguments.Arguments.Single(), Is.EqualTo("chrome"));
            Assert.That(firefoxArguments.Arguments.Single(), Is.EqualTo("firefox"));
        }

        [Test]
        public void Should_create_test_case_source_with_browser_suffix_for_each_browser_in_a_scenario_outline()
        {
            var chromeArguments = (TestCaseData[])BrowserRequiredTestFeature.ScenarioOutlineWithTwoBrowserTags_outline___chrome;
            var firefoxArguments = (TestCaseData[])BrowserRequiredTestFeature.ScenarioOutlineWithTwoBrowserTags_outline___firefox;

            Assert.That(chromeArguments.Count(), Is.EqualTo(2));
            Assert.That(firefoxArguments.Count(), Is.EqualTo(2));
            Assert.That(chromeArguments.First().Arguments.First(), Is.EqualTo("chrome"));
            Assert.That(chromeArguments.First().Arguments.Skip(1).First(), Is.EqualTo("other value"));
            Assert.That(firefoxArguments.First().Arguments.First(), Is.EqualTo("firefox"));
            Assert.That(firefoxArguments.First().Arguments.Skip(1).First(), Is.EqualTo("other value"));
        }

        [Test]
        public void Should_decorate_method_with_test_case_source_attribute_when_supplying_a_browser_tag_to_a_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseSourceAttribute>(() => this.Sample.MultipleBrowserTags("browser"));

            var firefox = testCaseAttributes.First(x => x.Category.Equals("firefox"));
            var chrome = testCaseAttributes.First(x => x.Category.Equals("chrome"));

            Assert.That(chrome.SourceName, Is.EqualTo("MultipleBrowserTags__chrome"));
            Assert.That(chrome.Category, Is.EqualTo("chrome"));

            Assert.That(firefox.SourceName, Is.EqualTo("MultipleBrowserTags__firefox"));
            Assert.That(firefox.Category, Is.EqualTo("firefox"));
        }

        [Test]
        public void Should_add_test_case_source_attribute_when_not_supplying_a_browser_to_a_scenario_outline()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseSourceAttribute>(() => this.Sample.TagsButNoBrowserTagScenarioOutline("header", null));

            var source = testCaseAttributes.Single();

            Assert.That(source.SourceName, Is.EqualTo("TagsButNoBrowserTagScenarioOutline_outline___no_browser"));
        }

        [Test]
        public void Should_add_one_test_case_per_browser_for_each_row_in_a_scenario_ouline_example()
        {
            Assert.That(BrowserRequiredTestFeature.ScenarioOutlineWithTwoBrowserTags_outline___chrome.Count(), Is.EqualTo(2));

            Assert.That(BrowserRequiredTestFeature.ScenarioOutlineWithTwoBrowserTags_outline___firefox.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Should_add_one_test_case_source_per_browser_in_a_scenario_outline_without_tagged_examples()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseSourceAttribute>(() => this.Sample.ScenarioOutlineWithTwoBrowserTags("browser", "header", null));

            Assert.That(testCaseAttributes.Count(), Is.EqualTo(2));
        }

		[Test]
		public void Should_add_one_test_case_source_per_tagged_example_test_data_set_in_a_scenario_outline()
		{
			var testCaseAttributes = this.GetMethodAttributes<TestCaseSourceAttribute>(() => this.Sample.ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples("browser", "header", null));

			Assert.That(testCaseAttributes.Count(), Is.EqualTo(6));
		}

        [Test, Ignore]
        public void Should_set_browser_tagged_description_to_scenario()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.SingleBrowserTagChrome("browser"));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "Single browser tag chrome" + " (chrome)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }

        [Test, Ignore]
        public void Should_set_browser_tagged_description_to_scenario_outline_testcase()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.ScenarioOutlineWithSingleBrowserTag("browser", "", null));

            var testCase = testCaseAttributes.SingleOrDefault();

            var expectedDescription = "scenario outline with single browser tag (scenario-outline-browser)";
            Assert.That(testCase.Description, Is.EqualTo(expectedDescription));
        }
    }
}