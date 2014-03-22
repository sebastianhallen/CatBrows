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
        public void Should_repeat_test_data_when_tagging_scenario_outline_with_repeat()
        {
            Assert.That(BrowserRequiredTestFeature.Repeated3TimesWith2OutlineValues_outline___browser.Count(), Is.EqualTo(6));
        }

        [Test]
        //@repeat:3
        public void Should_repeat_test_data_when_tagging_scenario_with_lower_case_repeat()
        {
            Assert.That(BrowserRequiredTestFeature.Repeat3TimesLowerCase__browser.Count(), Is.EqualTo(3));
        }

        [Test]
        //@Repeat:3
        public void Should_repeat_test_data_when_tagging_scenario_with_repeat()
        {
            Assert.That(BrowserRequiredTestFeature.Repeat3Times__browser.Count(), Is.EqualTo(3));
        }

        [Test]
        //@Repeats:3
        public void Should_repeat_test_data_when_tagging_scenario_with_repeats()
        {
            Assert.That(BrowserRequiredTestFeature.Repeats3Times__browser.Count(), Is.EqualTo(3));
        }

        [Test]
        //@Repeated:3
        public void Should_repeat_test_data_when_tagging_scenario_with_repeated()
        {
            Assert.That(BrowserRequiredTestFeature.Repeated3Times__browser.Count(), Is.EqualTo(3));
        }


    }
}