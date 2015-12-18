using System;
using System.Reflection;

namespace Jal.Aop.Aspects.Interface
{
    public interface IExceptionAdvice
    {
        object Handle(object[] arguments, Exception ex, MethodInfo method, object[] parameters, object target);
    }
}
