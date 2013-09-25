namespace CatBrows.Generator.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using NUnit.Framework;

    public abstract class GenerationTest
    {
        protected OverloadedBrowserTestFeature GeneratedTestCase;

        [SetUp]
        public void Before()
        {
            this.GeneratedTestCase = new OverloadedBrowserTestFeature();
        }

        [TearDown]
        public void After()
        {
            this.GeneratedTestCase.FeatureTearDown();
        }


        protected TAttribute[] GetMethodAttributes<TAttribute>(Expression<Action> expression)
            where TAttribute : Attribute
        {
            var methodBody = (MethodCallExpression)expression.Body;
            var methodInfo = methodBody.Method;

            var attributes = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
            return attributes.Cast<TAttribute>().ToArray();
        }
    }
}