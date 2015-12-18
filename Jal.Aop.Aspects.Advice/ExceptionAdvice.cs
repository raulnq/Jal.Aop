using System;
using System.Reflection;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Advice
{
    public class ExceptionAdvice : IExceptionAdvice
    {
        public object Handle(object[] arguments, Exception ex, MethodInfo method, object[] parameters, object target)
        {
            return method.ReturnType.IsValueType ? Activator.CreateInstance(method.ReturnType) : null;
        }
    }
}
