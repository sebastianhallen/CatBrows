namespace TestSample
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using NUnit.Framework;

    public abstract class GenerationTest<TSample>
        where TSample : class, new()
    {
        protected TSample Sample;

        [SetUp]
        public void BeforeGenerationTest()
        {
            this.Sample = new TSample();
            this.Sample.GetType().GetMethod("FeatureSetup").Invoke(this.Sample, new object[0]);
        }


        [TearDown]
        public void After()
        {
            this.Sample.GetType().GetMethod("FeatureTearDown").Invoke(this.Sample, new object[0]);
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