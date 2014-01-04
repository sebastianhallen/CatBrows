namespace CatBrows.Generator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using TestSample;
    using TestSample.DefaultSettings;

    [TestFixture]
    public class PropertiesTests
        : GenerationTest<BrowserRequiredTestFeature>
    {
        [Test]
        public void Should_add_colon_separated_tags_as_properties_to_test_method()
        {
            var properties = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.ScenarioOutlineWithTwoBrowserTagsAndTaggedExamples(null, null, null));

            var property = properties.Single();

            Assert.That(property.Properties["CustomProperty"], Is.EqualTo("PropertyValue"));
        }

        [Test]
        public void Should_add_colon_separated_tags_as_properties_to_class()
        {
            var properties = this.GetClassAttributes<BrowserRequiredTestFeature, PropertyAttribute>();

            var property = properties.Single();

            Assert.That(property.Properties["Config"], Is.EqualTo("Default"));
        }

        [Test]
        public void Should_not_include_duplicate_property_keys()
        {
            var properties = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.MultipleBrowserTags(null));

            Assert.That(properties.Any(), Is.False);
        }

        private TAttribute[] GetClassAttributes<T, TAttribute>()
            where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().ToArray();
        }
    }
}
