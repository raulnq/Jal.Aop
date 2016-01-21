using System;
using System.Linq;
using Castle.DynamicProxy;
using Castle.Windsor;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.CastleWindsor
{
    public class AopProxy : IInterceptor
    {
        private readonly Type[] _types;

        private readonly IPointCut _pointCut;

        private readonly IWindsorContainer _container;

        public AopProxy(Type[] types, IWindsorContainer container, IPointCut pointCut)
        {
            _pointCut = pointCut;
            _types = types;
            _container = container;
        }

        public void Intercept(IInvocation invocation)
        {
            var joinPoint = new JoinPoint
            {
                Arguments = invocation.Arguments,

                MethodInfo = invocation.GetConcreteMethodInvocationTarget(),

                ReturnValue = invocation.ReturnValue,

                TargetObject = invocation.InvocationTarget,

                TargetType = invocation.TargetType,

                ExecuteMethodFromProxy = (() =>
                {
                    invocation.Proceed();

                    return invocation.ReturnValue;
                }),

                SetReturnValueToProxy = ((o) => invocation.ReturnValue = o)
            };

            var typesToApply = _types.Where(x => _pointCut.CanApply(joinPoint, x)).ToArray();

            if (typesToApply.Length > 0)
            {
                var aspectsToApply = typesToApply.Select(x => _container.Resolve(x) as IAspect).ToArray();

                aspectsToApply = aspectsToApply.OrderBy(x => x.GetOrder(joinPoint)).ToArray();

                var root = aspectsToApply[0];

                var aspect = root;

                for (var i = 1; i < aspectsToApply.Length; i++)
                {
                    aspect.NextAspect = aspectsToApply[i];

                    aspect = aspect.NextAspect;
                }

                root.Apply(joinPoint);
            }
            else
            {
                invocation.Proceed();
            }

        }
    }
}