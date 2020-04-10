using Castle.DynamicProxy;

namespace Jal.Aop.CastleWindsor
{
    public class AopProxy : IInterceptor
    {
        private readonly IAspectExecutor _executor;

        public AopProxy(IAspectExecutor executor)
        {
            _executor = executor;
        }

        public void Intercept(IInvocation invocation)
        {
            var joinPoint = new JoinPoint
            {
                Arguments = invocation.Arguments,

                MethodInfo = invocation.GetConcreteMethodInvocationTarget(),

                Return = invocation.ReturnValue,

                TargetObject = invocation.InvocationTarget,

                TargetType = invocation.TargetType,

                ExecuteProxyInvocation = (jp =>
                {
                    invocation.Proceed();

                    jp.Return = invocation.ReturnValue;
                }),

                UpdateProxyInvocation = ((o) => invocation.ReturnValue = o)
            };

            _executor.Execute(joinPoint);
        }
    }
}