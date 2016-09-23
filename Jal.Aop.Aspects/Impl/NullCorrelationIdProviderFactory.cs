using System;
using System.Reflection;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Impl
{
    public class NullCorrelationIdProviderFactory : ICorrelationIdProviderFactory
    {
        public static NullCorrelationIdProviderFactory Instance = new NullCorrelationIdProviderFactory();
        public ICorrelationIdProvider Create(Type correlationIdProviderType)
        {
            return null;
        }
    }
}