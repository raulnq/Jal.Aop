using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Logger.Installer
{
    public class AspectLoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAspectLogger>().ImplementedBy<AspectLogger>().IsDefault());
        }

    }
}