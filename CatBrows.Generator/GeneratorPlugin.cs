using CatBrows.Generator;
using TechTalk.SpecFlow.Infrastructure;

[assembly: GeneratorPlugin(typeof(GeneratorPlugin))]
namespace CatBrows.Generator
{
	using BoDi;
	using TechTalk.SpecFlow.Generator.Configuration;
	using TechTalk.SpecFlow.Generator.Plugins;
	using TechTalk.SpecFlow.Generator.UnitTestProvider;

	public class GeneratorPlugin
        : IGeneratorPlugin
    {
        public void RegisterDependencies(ObjectContainer container)
        {
            
        }

        public void RegisterCustomizations(ObjectContainer container, SpecFlowProjectConfiguration generatorConfiguration)
        {
            container.RegisterTypeAs<NUnitBrowserTestGenerator, IUnitTestGeneratorProvider>();
        }

        public void RegisterConfigurationDefaults(SpecFlowProjectConfiguration specFlowConfiguration)
        {
            
        }
    }
}
