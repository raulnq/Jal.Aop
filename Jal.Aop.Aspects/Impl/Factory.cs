using System;
using Jal.Locator;

namespace Jal.Aop.Aspects
{
    public class Factory<T> : IFactory<T> where T : class
    {
        private readonly IServiceLocator _serviceLocator;

        public Factory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public T Create(IJoinPoint joinPoint, Type type)
        {
            return _serviceLocator.Resolve<T>(type.FullName);
        }
    }
}
