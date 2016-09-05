using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Aop.Aspects.Interface;
using Jal.Aop.Aspects.Serializer;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Serializer.Installer
{
    public class AspectSerializerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IAspectSerializer, AspectDataContractSerializer>(typeof(AspectDataContractSerializer).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IAspectSerializer, AspectXmlSerializer>(typeof(AspectXmlSerializer).FullName, new PerContainerLifetime());

            serviceRegistry.Register<IAspectSerializerFactory, AspectSerializerFactory>(new PerContainerLifetime());
        }
    }
}
