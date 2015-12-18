using System;

namespace Jal.Aop.Tests
{
    public class DumbClass : IDumbClass
    {
        [TestMethodBoundaryAspect(Order = 1)]
        [TestMethodBoundaryAspect2(Order = 2)]
        [TestMethodBoundaryAspect3(Order = 3)]
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
