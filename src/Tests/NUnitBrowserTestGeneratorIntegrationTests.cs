namespace CatBrows.Generator.Tests
{
    using NUnit.Framework;
    using System;
    using System.IO;
    using System.Reflection;
    using TechTalk.SpecFlow.Generator;
    using TechTalk.SpecFlow.Generator.Interfaces;
    using TechTalk.SpecFlow.Generator.Project;

    [TestFixture]
    public class NUnitBrowserTestGeneratorIntegrationTests
    {
        [Test]
        public void Should_be_able_to_construct_a_test_class()
        {
            const string testProject = "TestSample.DefaultSettings";
            const string featureFile = "BrowserRequiredTest.feature";
            var testProjectFolder = Path.Combine(AssemblyDirectory, @"..\..\..\", testProject);
            
            var configurationHolder = new SpecFlowConfigurationHolder(AppConfig);
            var projectSettings = new ProjectSettings
                {
                    AssemblyName =          testProject,
                    DefaultNamespace =      testProject,
                    ProjectName =           testProject,
                    ProjectFolder =         testProjectFolder,
                    ConfigurationHolder =   configurationHolder,
                    ProjectPlatformSettings=new ProjectPlatformSettings()
                };
            var specflowProject = new SpecFlowProject {ProjectSettings = projectSettings};
            var featurefileInput = specflowProject.GetOrCreateFeatureFile(featureFile);
            
            var container = GeneratorContainerBuilder.CreateContainer(configurationHolder, projectSettings);
            var generator = container.Resolve<ITestGenerator>();

            var testFile = generator.GenerateTestFile(featurefileInput, new GenerationSettings());
            var testFileContent = testFile.GeneratedTestCode;

            Console.WriteLine(testFileContent);
            Assert.That(testFile.Success);
        }

        private static readonly string AppConfig =
@"<specFlow>
    <unitTestProvider name=""NUnit""/>
    <plugins>
      <add name=""CatBrows"" path=""..\CatBrows.Generator\bin\Debug"" type=""Generator""/>
    </plugins>
    <stepAssemblies>
      <stepAssembly assembly=""TestSample"" />
    </stepAssemblies>
    <generator allowDebugGeneratedFiles=""true"" />
    <runtime missingOrPendingStepsOutcome=""Error"" />
</specFlow>";

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
