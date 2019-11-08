using System;
using Jal.Aop.Impl;

namespace Jal.Aop.Aspects
{
    public class LogAspectAttribute : AbstractAspectAttribute
    {
        public Type SerializerType { get; set; }

        public bool LogArguments { get; set; }

        public int[] SkipArguments { get; set; }

        public bool LogReturnValue { get; set; }

        public bool LogDuration { get; set; }

        public bool LogException { get; set; }

        public Type CorrelationIdProviderType { get; set; }

        public bool LogCorrelationId { get; set; }
        
        public string OnEntryMessageTemplate { get; set; }

        public string OnExitMessageTemplate { get; set; }

        public string OnExceptionMessageTemplate { get; set; }
    }
}
