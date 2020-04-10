using System;

namespace Jal.Aop.Aspects
{
    public interface IFactory<T>
    {
        T Create(IJoinPoint joinPoint, Type type);
    }
}
