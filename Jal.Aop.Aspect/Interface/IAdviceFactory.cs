using System;
using System.Reflection;

namespace Jal.Aop.Aspects.Interface
{
    public interface IAdviceFactory
    {
        IExceptionAdvice CreateExceptionAdvice(Exception ex, MethodInfo method, Type exceptionAdviceType);

        ISuccessAdvice CreateSuccessAdvice(MethodInfo method, Type successAdviceType);

        IEntryAdvice CreateEntryAdvice(MethodInfo method, Type entryAdviceType);

        IExitAdvice CreateExitAdvice(MethodInfo method, Type exitAdviceType);
    }
}
