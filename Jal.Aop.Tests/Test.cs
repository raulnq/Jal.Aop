using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Jal.Aop.Aspects.Installer;
using Jal.Aop.CastleWindsor;
using NUnit.Framework;

namespace Jal.Aop.Tests
{
    [TestFixture]
    public class Test
    {
        private IDumbClass _dumbClass;

        [SetUp]
        public void SetUp()
        {
            AssemblyFinder.Impl.AssemblyFinder.Current = new AssemblyFinder.Impl.AssemblyFinder(TestContext.CurrentContext.TestDirectory);
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new AutomaticInterception());
            container.Register(Component.For<IDumbClass>().ImplementedBy<DumbClass>());
            container.Install(new AspectInstaller());
            _dumbClass = container.Resolve<IDumbClass>();
        }

        [Test]
        public void OnEntryOnExit_WithSimpleMethod_Successful()
        {
            _dumbClass.PrintMessage("Hello");
            
        }
    }
}
