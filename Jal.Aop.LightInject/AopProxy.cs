using System;
using System.Linq;
using Jal.Aop.Impl;
using Jal.Aop.Interface;
using LightInject;
using LightInject.Interception;

namespace Jal.Aop.LightInject
{
    public class AopProxy : IInterceptor
    {
        private readonly Type[] _types;

        private readonly IPointCut _pointCut;

        private readonly IServiceContainer _container;

        public AopProxy(Type[] types, IServiceContainer container, IPointCut pointCut)
        {
            _pointCut = pointCut;
            _types = types;
            _container = container; 
        }

        public object Invoke(IInvocationInfo invocation)
        {

            var method = invocation.Proxy.Target.GetType().GetMethod(invocation.Method.Name);

            var joinPoint = new JoinPoint
            {
                Arguments = invocation.Arguments,

                MethodInfo = method,

                ReturnValue = null,

                TargetObject = invocation.Proxy.Target,

                TargetType = invocation.Proxy.Target.GetType(),

                ExecuteMethodFromProxy = (() =>
                {
                    var value = invocation.Proceed();

                    return value;
                }),
                
            };

            var typesToApply = _types.Where(x => _pointCut.CanApply(joinPoint, x)).ToArray();

            if (typesToApply.Length > 0)
            {
                var aspectsToApply = typesToApply.Select(x => _container.GetInstance(x) as IAspect).ToArray();

                aspectsToApply = aspectsToApply.OrderBy(x => x.GetOrder(joinPoint)).ToArray();

                var root = aspectsToApply[0];

                var aspect = root;

                for (var i = 1; i < aspectsToApply.Length; i++)
                {
                    aspect.NextAspect = aspectsToApply[i];

                    aspect = aspect.NextAspect;
                }

                root.Apply(joinPoint);

                return joinPoint.ReturnValue;
            }
            else
            {
                return invocation.Proceed();
            }

        }
    }
}