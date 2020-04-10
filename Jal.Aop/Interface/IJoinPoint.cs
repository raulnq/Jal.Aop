using System;
using System.Reflection;

namespace Jal.Aop
{
    public interface IJoinPoint
    {
        object[] Arguments { get; }

        MethodInfo MethodInfo { get; }

        object Return { get; set; }

        object TargetObject { get; }

        Type TargetType { get; }

        void Proceed();
    }
}
