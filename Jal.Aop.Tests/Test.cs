using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Jal.Aop.Aspects.Installer;
using Jal.Aop.CastleWindsor;
using Jal.Finder.Atrribute;
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
            var finder = Finder.Impl.AssemblyFinder.Create(TestContext.CurrentContext.TestDirectory);
           
            IWindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Kernel.ComponentModelBuilder.AddContributor(new AutomaticInterception());
            container.Register(Component.For<IDumbClass>().ImplementedBy<DumbClass>());
            container.Install(new AspectInstaller(finder.GetAssembliesTagged<AssemblyTagAttribute>()));
            _dumbClass = container.Resolve<IDumbClass>();
        }

        [Test]
        public void OnEntryOnExit_WithSimpleMethod_Successful()
        {
            _dumbClass.PrintMessage("Hello");
            
        }
    }
}
