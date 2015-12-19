using System;
using Cignium.Framework.Infrastructure.Aop.Aspects.Interface;

namespace Cignium.Framework.Infrastructure.Aop.Aspects.Impl
{
    public class NullReturnFailureValueCreator : IReturnFailureValueCreator
    {
        public object Create(Type type, string[] messages)
        {
            return Activator.CreateInstance(type);
        }
    }
}
