using System;
using Common.Logging;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Logger
{
    public class AspectLogger : IAspectLogger
    {
        private readonly ILog _log;

        public AspectLogger(ILog log)
        {
            _log = log;
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Error(Exception exception)
        {
            _log.Error(exception);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }
    }
}
