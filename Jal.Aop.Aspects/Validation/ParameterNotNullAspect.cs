using System.Linq;
using Castle.DynamicProxy;
using Cignium.Framework.Infrastructure.Aop.Aspects.Interface;

namespace Cignium.Framework.Infrastructure.Aop.Aspects.Validation
{
    public class ParameterNotNullAspect : OnMethodBoundaryAspect
    {
        private const string NotNullErrorMessage = "Parameters cannot be null";

        private readonly IReturnFailureValueCreator _returnFailureValueCreator;

        public ParameterNotNullAspect(IReturnFailureValueCreator returnFailureValueCreator)
        {
            _returnFailureValueCreator = returnFailureValueCreator;

            HandleException = false;
        }

        protected override bool Continue(IInvocation invocation)
        {
            if (invocation.Arguments.Any(argument => argument == null))
            {
                invocation.ReturnValue = _returnFailureValueCreator.Create(invocation.MethodInvocationTarget.ReturnType,new [] {NotNullErrorMessage});

                return false;
            }
            return true;
        }
    }
}
