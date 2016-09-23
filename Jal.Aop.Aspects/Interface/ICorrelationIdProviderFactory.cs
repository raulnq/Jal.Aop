using System;

namespace Jal.Aop.Aspects.Interface
{
    public interface ICorrelationIdProviderFactory
    {
        ICorrelationIdProvider Create(Type correlationIdProviderType);
    }
}