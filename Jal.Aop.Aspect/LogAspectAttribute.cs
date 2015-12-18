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

        public bool LogException { get; set; }
    }
}
