namespace CatBrows.Generator.Tests
{
    using System;
    using System.Collections;
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
            const string propertyKey = "Config";

            var value = this.GetClassPropertyValue(propertyKey);

            Assert.That(value, Is.EqualTo("Default"));
        }

        [Test]
        public void Should_be_possible_to_have_multiple_values_for_scenario_properties()
        {
            var properties = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.MultipleBrowserTags(null));

            var duplicatePropertyValue = properties.Single().Properties;
            var key = duplicatePropertyValue.Keys.OfType<string>().Single();
            var value = duplicatePropertyValue.Values.OfType<string>().Single();

            Assert.That(key, Is.EqualTo("Duplicate"));
            Assert.That(value, Is.EqualTo("Property0,Property1"));
        }

        [Test]
        public void Should_be_possible_to_have_multiple_values_for_class_properties()
        {
            var properties = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.MultipleBrowserTags(null));

            var duplicatePropertyValue = properties.Single().Properties;
            var key = duplicatePropertyValue.Keys.OfType<string>().Single();
            var value = duplicatePropertyValue.Values.OfType<string>().Single();

            Assert.That(key, Is.EqualTo("Duplicate"));
            Assert.That(value, Is.EqualTo("Property0,Property1"));
        }

        [Test]
        public void Scenario_property_values_should_be_able_to_contain_values_not_allowed_for_categories()
        {
            var property = this.GetMethodAttributes<PropertyAttribute>(() => this.Sample.ScenarioWithPropertyValueNotAllowedForCategories("_")).Single();

            var value = property.Properties.Values.OfType<string>().Single();

            Assert.That(value, Is.EqualTo("Property-With-Illegal-Chars-For-Category-+-!,"));
        }

        [Test]
        public void Feature_property_values_should_be_able_to_contain_values_not_allowed_for_categories()
        {
            const string propertyKey = "Feature-PropertyWithIllegalCategoryChars";

            var value = this.GetClassPropertyValue(propertyKey);

            Assert.That(value, Is.EqualTo("Property-With-Illegal-Chars-For-Category-+-!,"));
        }

        private string GetClassPropertyValue(string propertyKey)
        {
            var properties = this.GetClassAttributes<BrowserRequiredTestFeature, PropertyAttribute>()
                                 .Single(p => p.Properties.Keys.OfType<string>().Any(key => key.Equals(propertyKey)));
            var value = properties.Properties[propertyKey] as string;
            return value;
        }

        private TAttribute[] GetClassAttributes<T, TAttribute>()
            where TAttribute : Attribute
        {
            return typeof(T).GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().ToArray();
        }
    }
}
