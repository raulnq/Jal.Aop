using System;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Tests
{
    public class TestMethodBoundaryAspect : OnMethodBoundaryAspect<TestMethodBoundaryAspectAttribute>
    {
        protected override void OnEntry(IJoinPoint invocation)
        {
            Console.WriteLine("OnEntry " + invocation.MethodInfo);
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            Console.WriteLine("OnExit "+ invocation.MethodInfo);
        }
    }

    public class TestMethodBoundaryAspectAttribute : AbstractAspectAttribute
    {
        
    }
}
