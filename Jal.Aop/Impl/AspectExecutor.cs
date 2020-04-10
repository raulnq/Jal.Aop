using System;
using System.Linq;
using Jal.Locator;

namespace Jal.Aop
{
    public class AspectExecutor : IAspectExecutor
    {
        private readonly Type[] _types;

        private readonly IPointCut _pointCut;

        private readonly IServiceLocator _locator;

        public AspectExecutor(Type[] types, IServiceLocator locator, IPointCut pointCut)
        {
            _locator = locator;
            _pointCut = pointCut;
            _types = types;
        }

        public void Execute(IJoinPoint joinPoint)
        {
            var typesToApply = _types.Where(x => _pointCut.CanApply(joinPoint, x));

            if (typesToApply.Count() > 0)
            {
                var aspectsToApply = typesToApply.Select(x => _locator.Resolve<IAspect>(x.FullName)).OrderBy(x => x.GetOrder(joinPoint)).ToArray();

                var root = aspectsToApply[0];

                var aspect = root;

                for (var i = 1; i < aspectsToApply.Length; i++)
                {
                    aspect.SetNext(aspectsToApply[i]);

                    aspect = aspect.GetNext();
                }

                root.Apply(joinPoint);
            }
            else
            {
                joinPoint.Proceed();
            }
        }
    }
}
