using System;

namespace Jal.Aop
{
    public interface IPointCut
    {
        bool CanApply(IJoinPoint joinPoint, Type type);
    }
}
