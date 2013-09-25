﻿namespace CatBrows.Generator.Tests
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class CategoryTests
        : GenerationTest
    {
        [Test]
        public void Should_add_browser_as_category_to_scenario_testrows()
        {
            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.GeneratedTestCase.MultipleBrowserTags("browser"));

            var chromeCase = testCaseAttributes.Single(attr => "chrome".Equals((string) attr.Category));
            var firefoxCase = testCaseAttributes.Single(attr => "firefox".Equals(attr.Category));

            Assert.That(chromeCase.Categories.Count, Is.EqualTo(1), "Chrome was not set as category");
            Assert.That(firefoxCase.Categories.Count, Is.EqualTo(1), "Firefox was not set as category");
        }

        [Test]
        public void Should_include_scenario_outline_example_tags_in_test_case_row_categories()
        {
            var rows = this.GetMethodAttributes<TestCaseAttribute>(() =>
                                                                   this.GeneratedTestCase.ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples("browser", "header", null)).ToArray();

            var tagCombinations = rows.Select(row => row.Categories.OfType<string>());

            Assert.That(tagCombinations, Is.EquivalentTo(new[]
                {
                    new [] {"nightly", "chrome" },
                    new [] {"nightly", "firefox" },
                    new [] {"each-commit", "chrome" },
                    new [] {"each-commit", "firefox" }
                }));
        }
    }
}