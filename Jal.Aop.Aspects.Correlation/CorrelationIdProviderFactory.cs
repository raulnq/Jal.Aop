using System;
using Jal.Aop.Aspects.Interface;
using Jal.Locator.Interface;

namespace Jal.Aop.Aspects.Correlation
{
    public class CorrelationIdProviderFactory : ICorrelationIdProviderFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public CorrelationIdProviderFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        public ICorrelationIdProvider Create(Type correlationIdProviderType)
        {
            return correlationIdProviderType != null ? _serviceLocator.Resolve<ICorrelationIdProvider>(correlationIdProviderType.FullName) : null;
        }
    }
}
