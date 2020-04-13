using System.Reflection;

namespace Jal.Aop.DispatchProxy
{
    public class AopProxy<TService, TImplementation> : System.Reflection.DispatchProxy where TImplementation : TService
    {
        private IAspectExecutor _executor;

        private TService _target;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var method = typeof(TImplementation).GetMethod(targetMethod.Name);

            var joinPoint = new JoinPoint
            {
                Arguments = args,

                MethodInfo = method,

                Return = targetMethod.ReturnType,

                TargetObject = _target,

                TargetType = typeof(TImplementation),

                ExecuteProxyInvocation = (jp =>
                {
                    var result = targetMethod.Invoke(_target, args);

                    jp.Return = result;
                }),
            };

            _executor.Execute(joinPoint);

            return joinPoint.Return;
        }

        public void Init(TService target, IAspectExecutor executer)
        {
            _executor = executer;

            _target = target;
        }
    }
}
