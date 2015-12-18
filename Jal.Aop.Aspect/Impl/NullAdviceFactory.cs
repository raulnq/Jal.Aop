using System;
using System.Reflection;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Impl
{
    public class NullAdviceFactory : IAdviceFactory
    {
        public static NullAdviceFactory Instance = new NullAdviceFactory();

        public IExceptionAdvice CreateExceptionAdvice(Exception ex, MethodInfo method, Type exceptionAdviceType)
        {
            return null;
        }

        public ISuccessAdvice CreateSuccessAdvice(MethodInfo method, Type successAdviceType)
        {
            return null;
        }

        public IEntryAdvice CreateEntryAdvice(MethodInfo method, Type entryAdviceType)
        {
            return null;
        }

        public IExitAdvice CreateExitAdvice(MethodInfo method, Type exitAdviceType)
        {
            return null;
        }
    }
}
