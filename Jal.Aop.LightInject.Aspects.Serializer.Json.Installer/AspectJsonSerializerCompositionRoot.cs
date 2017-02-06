using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Aop.Aspects.Interface;
using Jal.Aop.Aspects.Serializer.Json;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Serializer.Json.Installer
{
    public class AspectJsonSerializerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IAspectSerializer, AspectJsonSerializer>(typeof(AspectJsonSerializer).FullName, new PerContainerLifetime());
        }
    }
}
