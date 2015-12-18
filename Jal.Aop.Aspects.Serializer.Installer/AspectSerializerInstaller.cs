using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Serializer.Installer
{
    public class AspectSerializerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAspectSerializer>().ImplementedBy<AspectDataContractSerializer>().Named(typeof(AspectDataContractSerializer).FullName).IsDefault());
            container.Register(Component.For<IAspectSerializer>().ImplementedBy<AspectXmlSerializer>().Named(typeof(AspectXmlSerializer).FullName));
            container.Register(Component.For<IAspectSerializerFactory>().ImplementedBy<AspectSerializerFactory>());
        }

    }
}