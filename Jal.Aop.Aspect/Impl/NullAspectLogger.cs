using System;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Impl
{
    public class NullAspectLogger : IAspectLogger
    {
        public static NullAspectLogger Instance = new NullAspectLogger();

        public void Info(string message)
        {
            
        }

        public void Error(Exception exception)
        {
            
        }
    }
}
