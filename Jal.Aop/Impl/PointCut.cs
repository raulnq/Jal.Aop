using System;
using System.Linq;
using Jal.Aop.Interface;

namespace Jal.Aop.Impl
{
    public class PointCut : IPointCut
    {
        public bool CanApply(IJoinPoint joinPoint, Type aspectType)
        {
            if (aspectType.BaseType.IsGenericType)
            {
                var attibuteTypes = aspectType.BaseType.GetGenericArguments();
                if (attibuteTypes.Length > 0)
                {
                    var attibuteType = attibuteTypes.FirstOrDefault(x => typeof(AbstractAspectAttribute).IsAssignableFrom(x));
                    if (attibuteType != null)
                    {
                        var attributes = joinPoint.MethodInfo.GetCustomAttributes(attibuteType, true);
                        return attributes.Length > 0;
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return true;
            }
        }
    }
}
