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

        //[Test]
        //public void Should_be_possible_to_have_multiple_values_for_properties()
        //{
        //    var properties = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.MultipleBrowserTags(null));

        //    var duplicatePropertyValue = properties.Single().Properties;
        //    var key = duplicatePropertyValue.Keys.OfType<string>().Single();
        //    var value = duplicatePropertyValue.Values.OfType<string>().Single();

        //    Assert.That(key, Is.EqualTo("Duplicate"));
        //    Assert.That(value, Is.EquivalentTo(new[] { "Property0", "Property1" }));
        //}

        private TAttribute[] GetClassAttributes<T, TAttribute>()
            where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().ToArray();
        }
    }
}
