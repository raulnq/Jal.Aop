using Common.Logging;
using Jal.Aop.LightInject.Aspect.Installer;
using Jal.Aop.LightInject.Aspects.Logger.Installer;
using Jal.Aop.LightInject.Aspects.Serializer.Installer;
using Jal.Finder.Atrribute;
using Jal.Locator.LightInject.Impl;
using Jal.Locator.LightInject.Installer;
using LightInject;
using NUnit.Framework;

namespace Jal.Aop.LightInject.Tests
{
    [TestFixture]
    public class Test
    {
        private IDumbClass _dumbClass;

        [SetUp]
        public void SetUp()
        {
            var finder = Finder.Impl.AssemblyFinder.Create(TestContext.CurrentContext.TestDirectory);

            var container = new ServiceContainer();

            container.RegisterFrom<ServiceLocatorCompositionRoot>();

            container.RegisterAspects(finder.GetAssembliesTagged<AssemblyTagAttribute>());

            container.Register<IDumbClass, DumbClass>();

            var logger = LogManager.GetLogger<Test>();

            container.Register<ILog>(x => logger);

            container.RegisterFrom<AspectLoggerCompositionRoot>();

            container.RegisterFrom<AspectSerializerCompositionRoot>();

            container.RegisterFrom<AutomaticInterceptionCompositionRoot>();

            _dumbClass = container.GetInstance<IDumbClass>();
        }

        [Test]
        public void OnEntryOnExit_WithSimpleMethod_Successful()
        {
            _dumbClass.PrintMessage("Hello");
            
        }
    }
}
