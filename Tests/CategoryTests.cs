namespace CatBrows.Generator.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using TestSample;
    using TestSample.DefaultSettings;

    [TestFixture]
    public class CategoryTests
        : GenerationTest<BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_add_browser_as_category_to_scenario_testrows()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.Sample.MultipleBrowserTags("browser"));

            var chromeCase = testCaseAttributes.Single(attr => "chrome".Equals((string)attr.Category));
            var firefoxCase = testCaseAttributes.Single(attr => "firefox".Equals(attr.Category));

            Assert.That(chromeCase.Categories.Count, Is.EqualTo(1), "Chrome was not set as category");
            Assert.That(firefoxCase.Categories.Count, Is.EqualTo(1), "Firefox was not set as category");
        }

        [Test]
        public void Should_include_scenario_outline_example_tags_in_test_case_row_categories()
        {
            var rows = this.GetMethodAttributes<TestCaseAttribute>(() =>
                                                                   this.Sample.ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples("browser", "header", null)).ToArray();

            var tagCombinations = rows.Select(row => row.Categories.OfType<string>());

            Assert.That(tagCombinations, Is.EquivalentTo(new[]
                {
                    new [] {"nightly", "chrome" },
                    new [] {"nightly", "firefox" },
                    new [] {"each-commit", "chrome" },
                    new [] {"each-commit", "firefox" }
                }));
        }

        [Test]
        public void Should_add_class_name_as_category_to_fixture()
        {
            var fixtureCategories = this.GetClassAttributes<BrowserRequiredTestFeature, CategoryAttribute>();

            Assert.That(fixtureCategories.Any(cat => cat.Name.Equals("BrowserRequiredTestFeature")));
        }

        [Test]
        public void Should_add_last_part_of_namespace_as_category_to_fixture()
        {
            var fixtureCategories = this.GetClassAttributes<BrowserRequiredTestFeature, CategoryAttribute>();

            Assert.That(fixtureCategories.Any(cat => cat.Name.Equals("DefaultSettings")));

        }

        private TAttribute[] GetClassAttributes<T, TAttribute>()
            where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().ToArray();
        }
    }
}