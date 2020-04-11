using System;

namespace Jal.Aop.Aspects
{
    public interface ILogger
    {
        void OnExit(IJoinPoint joinpoint, Return @return, string requestid, long duration);

        void OnExit(IJoinPoint joinpoint, string requestid, long duration);

        void OnExit(IJoinPoint joinpoint, Return @return, string requestid);

        void OnExit(IJoinPoint joinpoint, string requestid);

        void OnEntry(IJoinPoint joinpoint, Argument[] arguments, string requestid);

        void OnException(IJoinPoint joinpoint, string requestid, Exception ex);
    }
}
