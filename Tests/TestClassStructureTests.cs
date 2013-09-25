namespace CatBrows.Generator.Tests
{
    using NUnit.Framework;
    using System;
    using TechTalk.SpecFlow;

    [TestFixture]
    public class TestClassStructureTests
        : GenerationTest
    {
        [Test]
        public void Should_tag_class_with_testfixture_attribute()
        {
            var fixtureAttribute = Attribute.GetCustomAttribute(typeof(BrowserTestFeature), typeof(TestFixtureAttribute));

            Assert.That(fixtureAttribute, Is.Not.Null);
        }

        [Test]
        public void Should_initialize_browser_when_running_browser_tagged_scenario()
        {
            this.GeneratedTestCase.SingleBrowserTagChrome("some browser");

            Assert.That(ScenarioContext.Current["Browser"], Is.EqualTo("some browser"));
        }

        [Test]
        public void Browser_should_be_set_when_background_is_invoked()
        {
            this.GeneratedTestCase.SingleBrowserTagChrome("some browser");

            Assert.That(BrowserTestSteps.BackgroundHasBrowser);
        }
    }
}
