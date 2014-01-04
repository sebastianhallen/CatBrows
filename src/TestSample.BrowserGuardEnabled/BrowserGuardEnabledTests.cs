namespace TestSample.BrowserGuardEnabled
{
    using System;
    using NUnit.Framework;
    using TestSample;

    [TestFixture]
    public class BrowserGuardEnabledTests
        : GenerationTest<TestSample.BrowserGuardEnabled.BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_throw_exception_when_no_tags_at_all_are_supplied()
        {
            this.VerifyNoBrowserDefinedException(() => this.Sample.NoTagsAtAll());
        }

        [Test]
        public void Should_throw_exception_when_no_browser_tag_is_supplied_with_other_tags()
        {
            this.VerifyNoBrowserDefinedException(() => this.Sample.TagsButNoBrowserTag());
        }

        [Test]
        public void Should_throw_no_browser_defined_exception_for_scenario_outlines_without_tags()
        {
            this.VerifyNoBrowserDefinedException(() => this.Sample.NoTagsScenarioOutline("some value", null));
        }

        [Test]
        public void Should_throw_no_browser_defined_exception_for_scenario_outlines_with_tags_but_without_browser_tags()
        {
            this.VerifyNoBrowserDefinedException(() => this.Sample.TagsButNoBrowserTagScenarioOutline("some value", null));
        }

        private void VerifyNoBrowserDefinedException(Action action)
        {
            var exception = Assert.Throws<Exception>(() => action());

            Assert.That(exception.Message, Is.EqualTo("Oh noes, there is no @Browser-tag present."));
        }
    }
}