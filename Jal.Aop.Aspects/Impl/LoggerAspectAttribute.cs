using System;

namespace Jal.Aop.Aspects
{
    public class LoggerAspectAttribute : AbstractAspectAttribute
    {
        public Type LoggerType { get; set; }

        public Type SerializerType { get; set; }

        public bool LogArguments { get; set; }

        public int[] SkipArguments { get; set; }

        public bool LogReturn { get; set; }

        public bool LogDuration { get; set; }

        public bool LogException { get; set; }

        public Type RequestIdProviderType { get; set; }

        public string OnEntryMessageTemplate { get; set; }

        public string OnExitMessageTemplate { get; set; }

        public string OnExceptionMessageTemplate { get; set; }
    }
}
