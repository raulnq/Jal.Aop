using System;
using Jal.Aop.Aspects.Interface;
using Jal.Locator.Interface;

namespace Jal.Aop.Aspects.Serializer
{
    public class AspectSerializerFactory : IAspectSerializerFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public AspectSerializerFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IAspectSerializer Create(Type aspectSerializerType)
        {
            return aspectSerializerType != null ? _serviceLocator.Resolve<IAspectSerializer>(aspectSerializerType.FullName) : _serviceLocator.Resolve<IAspectSerializer>();
        }
    }
}
