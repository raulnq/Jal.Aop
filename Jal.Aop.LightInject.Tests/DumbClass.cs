using System;
using Jal.Aop.Aspects;
using Jal.Aop.Aspects.Serializer;

namespace Jal.Aop.LightInject.Tests
{
    public class DumbClass : IDumbClass
    {
        //[TestMethodBoundaryAspect(Order = 1)]
        //[TestMethodBoundaryAspect2(Order = 2)]
        //[TestMethodBoundaryAspect3(Order = 3)]
        [LogAspect(Order = 1, LogArguments = true, SerializerType = typeof(AspectDataContractSerializer))]
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

    public interface IDumbClass
    {
        void PrintMessage(string message);
    }
}
