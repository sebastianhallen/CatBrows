//namespace Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Linq.Expressions;
//    using BrowserTestGenerator;
//    using NUnit.Framework;
//    using TechTalk.SpecFlow;
//    using TechTalk.SpecFlow.Generator;
//    using TechTalk.SpecFlow.Utils;

//    [TestFixture]
//    public class IntegrationTests
//    {
//        private OverloadedBrowserTestFeature generatedTestCase;

//        [SetUp]
//        public void Before()
//        {
//            this.generatedTestCase = new OverloadedBrowserTestFeature();
//        }

//        [Test]
//        public void Should_tag_class_with_testfixture_attribute()
//        {
//            var fixtureAttribute = Attribute.GetCustomAttribute(typeof(BrowserTestFeature), typeof(TestFixtureAttribute));

//            Assert.That(fixtureAttribute, Is.Not.Null);
//        }

//        [Test]
//        public void Should_throw_exception_when_no_tags_at_all_are_supplied()
//        {
//            Assert.Throws<NoBrowserDefinedException>(() => this.generatedTestCase.NoTagsAtAll());
//        }

//        [Test]
//        public void Should_throw_exception_when_no_browser_tag_is_supplied_with_other_tags()
//        {
//            Assert.Throws<NoBrowserDefinedException>(() => this.generatedTestCase.TagsButNoBrowserTag());
//        }

//        [Test]
//        public void Should_add_each_browser_as_category_to_scenario_methods()
//        {
//            var categories = this.GetMethodAttributes<CategoryAttribute>(() => this.generatedTestCase.MultipleBrowserTags("browser"));

//            var chrome = categories.SingleOrDefault(category => category.Name.Equals("chrome"));
//            var firefox = categories.SingleOrDefault(category => category.Name.Equals("firefox"));

//            Assert.That(chrome, Is.Not.Null, "Chrome was not set as category");
//            Assert.That(firefox, Is.Not.Null, "Firefox was not set as category");
//        }

//        [Test]
//        public void Should_decorate_method_with_test_case_attribute_when_supplying_a_browser_tag_to_a_scenario()
//        {
//            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.SingleBrowserTagChrome("browser"));

//            var tca = testCaseAttributes.SingleOrDefault();
//            Assert.That(tca, Is.Not.Null);
//            Assert.That(tca.Arguments.Single(), Is.EqualTo("chrome"));
//        }

//        [Test]
//        public void Should_decorate_method_with_test_case_attributes_for_each_browser_tag_when_supplying_multiple_browser_tags_to_a_scenario()
//        {
//            var testCaseAttributes = this.GetMethodAttributes<TestCaseAttribute>(() => this.generatedTestCase.MultipleBrowserTags("browser"));

//            var chrome = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "chrome"));
//            var firefox = testCaseAttributes.SingleOrDefault(a => Equals(a.Arguments.Single(), "firefox"));
            
//            Assert.That(chrome, Is.Not.Null);
//            Assert.That(firefox, Is.Not.Null);
//        }

//        [Test]
//        public void Should_initialize_browser_when_running_browser_tagged_scenario()
//        {
//            this.generatedTestCase.SingleBrowserTagChrome("some browser");

//            Assert.That(ScenarioContext.Current["Browser"], Is.EqualTo("some browser"));
//        }

//        private TAttribute[] GetMethodAttributes<TAttribute>(Expression<Action> expression)
//            where TAttribute : Attribute
//        {
//            var methodBody = (MethodCallExpression)expression.Body;
//            var methodInfo = methodBody.Method;

//            var attributes = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
//            return attributes.Cast<TAttribute>().ToArray();
//        }
//    }
//}
