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
            var testProject = "TestSample.DefaultSettings";
            var testProjectFolder = Path.Combine(AssemblyDirectory, @"..\..\..\", testProject);
            var featureFile = "BrowserRequiredTest.feature";

            var configurationHolder = new SpecFlowConfigurationHolder(AppConfig);
            
            var projectSettings = new ProjectSettings
                {
                    AssemblyName = testProject,
                    ConfigurationHolder = configurationHolder,
                    DefaultNamespace = testProject,
                    ProjectFolder = testProjectFolder,
                    ProjectName = testProject,
                    ProjectPlatformSettings = new ProjectPlatformSettings()
                };
            var specflowProject = new SpecFlowProject {ProjectSettings = projectSettings};
            var container = GeneratorContainerBuilder.CreateContainer(configurationHolder, projectSettings);
            var generator = container.Resolve<ITestGenerator>();

            var featurefileInput = specflowProject.GetOrCreateFeatureFile(featureFile);
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
