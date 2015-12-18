using System;

namespace Cignium.Framework.Infrastructure.Aop.Aspects.Interface
{
    public interface IReturnFailureValueCreator
    {
        object Create(Type type, string[] messages);
    }
}
