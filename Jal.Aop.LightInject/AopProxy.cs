using LightInject.Interception;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Aop.LightInject
{
    public class AopProxy : IInterceptor
    {
        private readonly IAspectExecutor _executor;

        public AopProxy(IAspectExecutor executor)
        {
            _executor = executor; 
        }

        public object Invoke(IInvocationInfo invocation)
        {
            var method = invocation.Proxy.Target.GetType().GetMethod(invocation.Method.Name);

            var joinPoint = new JoinPoint
            {
                Arguments = invocation.Arguments,

                MethodInfo = method,

                Return = null,

                TargetObject = invocation.Proxy.Target,

                TargetType = invocation.Proxy.Target.GetType(),

                ExecuteProxyInvocation = (jp) =>
                {
                    jp.Return = invocation.Proceed();
                },
                
            };

            _executor.Execute(joinPoint);

            return joinPoint.Return;
        }
    }
}