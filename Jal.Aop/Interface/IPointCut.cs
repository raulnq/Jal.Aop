using System;

namespace Jal.Aop.Interface
{
    public interface IPointCut
    {
        bool CanApply(IJoinPoint joinPoint, Type aspectType);
    }
}
