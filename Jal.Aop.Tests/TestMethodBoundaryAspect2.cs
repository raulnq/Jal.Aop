using System;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Tests
{
    public class TestMethodBoundaryAspect2 : OnMethodBoundaryAspect<TestMethodBoundaryAspect2Attribute>
    {
        protected override void OnEntry(IJoinPoint invocation)
        {
            Console.WriteLine("OnEntry2 " + invocation.MethodInfo);
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            Console.WriteLine("OnExit2 "+ invocation.MethodInfo);
        }
    }

    public class TestMethodBoundaryAspect2Attribute : AbstractAspectAttribute
    {

    }
}
