using System;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Impl
{
    public class NullAspectSerializerFactory : IAspectSerializerFactory
    {
        public static NullAspectSerializerFactory Instance = new NullAspectSerializerFactory();

        public IAspectSerializer Create(Type aspectSerializerType)
        {
            return null;
        }
    }
}
