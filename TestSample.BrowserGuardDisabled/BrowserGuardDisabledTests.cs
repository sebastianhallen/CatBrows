namespace TestSample.BrowserGuardDisabled
{
    using NUnit.Framework;
    using System;
    using TestSample;

    [TestFixture]
    public class BrowserGuardDisabledTests
        : GenerationTest<BrowserOptionalTestFeature>
    {
        [Test]
        public void Should_not_throw_exception_when_no_tags_at_all_are_supplied()
        {
            this.Sample.NoTagsAtAll();
        }

        [Test]
        public void Should_not_throw_exception_when_no_browser_tag_is_supplied_with_other_tags()
        {
            this.Sample.TagsButNoBrowserTag();
        }

        [Test]
        public void Should_not_throw_no_browser_defined_exception_for_scenario_outlines_without_tags()
        {
            this.Sample.NoTagsScenarioOutline("some value", null);
        }

        [Test]
        public void Should_not_throw_no_browser_defined_exception_for_scenario_outlines_with_tags_but_without_browser_tags()
        {
            this.Sample.TagsButNoBrowserTagScenarioOutline("some value", null);
        }
    }
}
