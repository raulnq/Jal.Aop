using System;
using Jal.Aop.Impl;

namespace Jal.Aop.Aspects
{
    public class AroundMethodAspectAttribute : AbstractAspectAttribute
    {
        public Type SuccessAdviceType { get; set; }

        public Type ExceptionAdviceType { get; set; }

        public Type EntryAdviceType { get; set; }

        public Type ExitAdviceType { get; set; }

        public object[] Context { get; set; }
    }
}
