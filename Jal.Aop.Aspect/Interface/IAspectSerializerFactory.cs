using System;

namespace Jal.Aop.Aspects.Interface
{
    public interface IAspectSerializerFactory
    {
        IAspectSerializer Create(Type aspectSerializerType);
    }
}
