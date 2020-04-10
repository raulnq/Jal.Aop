using System;

namespace Jal.Aop.Aspects
{
    public class Advice : IAdvice
    {
        public virtual void OnEntry(IJoinPoint joinpoint, object[] context)
        {

        }

        public virtual void OnException(IJoinPoint joinpoint, object[] context, Exception ex)
        {
            if(joinpoint.MethodInfo.ReturnType.IsValueType)
            {
                joinpoint.Return = Activator.CreateInstance(joinpoint.MethodInfo.ReturnType);
            }
        }

        public virtual void OnExit(IJoinPoint joinpoint, object[] context)
        {

        }

        public virtual void OnSuccess(IJoinPoint joinpoint, object[] context)
        {

        }
    }
}
