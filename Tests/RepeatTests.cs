namespace CatBrows.Generator.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using TestSample;
    using TestSample.DefaultSettings;

    [TestFixture]
    public class RepeatTests
        : GenerationTest<BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_repeat_test_data_when_tagging_scenario_with_repeat()
        {
            Assert.That(BrowserRequiredTestFeature.Repeated3Times__browser.Count(), Is.EqualTo(3));
        }

        [Test]
        public void Should_repeat_test_data_when_tagging_scenario_outline_with_repeat()
        {
            Assert.That(BrowserRequiredTestFeature.Repeated3TimesWith2OutlineValues_outline___browser.Count(), Is.EqualTo(6));
        }
    }
}