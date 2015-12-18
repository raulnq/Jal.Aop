using System;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Tests
{
    public class TestMethodBoundaryAspect3 : OnMethodBoundaryAspect<TestMethodBoundaryAspect3Attribute>
    {
        protected override void OnEntry(IJoinPoint invocation)
        {
            Console.WriteLine("OnEntry3 " + invocation.MethodInfo);
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            Console.WriteLine("OnExit3 "+ invocation.MethodInfo);
        }
    }
    public class TestMethodBoundaryAspect3Attribute : AbstractAspectAttribute
    {

    }
}
