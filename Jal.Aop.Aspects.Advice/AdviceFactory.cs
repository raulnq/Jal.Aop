using System;
using System.Reflection;
using Jal.Aop.Aspects.Interface;
using Jal.Locator.Interface;

namespace Jal.Aop.Aspects.Advice
{
    public class AdviceFactory : IAdviceFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public AdviceFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IExceptionAdvice CreateExceptionAdvice(Exception ex, MethodInfo method, Type exceptionAdviceType)
        {
            return exceptionAdviceType!=null ? _serviceLocator.Resolve<IExceptionAdvice>(exceptionAdviceType.FullName) : null;
        }

        public ISuccessAdvice CreateSuccessAdvice(MethodInfo method, Type successAdviceType)
        {
            return successAdviceType != null ? _serviceLocator.Resolve<ISuccessAdvice>(successAdviceType.FullName) : null;
        }

        public IEntryAdvice CreateEntryAdvice(MethodInfo method, Type entryAdviceType)
        {
            return entryAdviceType != null ? _serviceLocator.Resolve<IEntryAdvice>(entryAdviceType.FullName) : null;
        }

        public IExitAdvice CreateExitAdvice(MethodInfo method, Type exitAdviceType)
        {
            return exitAdviceType != null ? _serviceLocator.Resolve<IExitAdvice>(exitAdviceType.FullName) : null;
        }
    }
}
