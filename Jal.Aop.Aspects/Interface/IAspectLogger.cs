using System;

namespace Jal.Aop.Aspects.Interface
{
    public interface IAspectLogger
    {
        void Info(string message);

        void Error(Exception exception);

        void Error(string message, Exception exception);
    }
}
