using System;
using System.Reflection;

namespace Jal.Aop.Interface
{
    public interface IJoinPoint
    {
        object[] Arguments { get; }

        MethodInfo MethodInfo { get; }

        object ReturnValue { get; set; }

        object TargetObject { get; }

        Type TargetType { get; }

        void Proceed();
    }
}
