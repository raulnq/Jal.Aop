using System;

namespace Jal.Aop.Impl
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AbstractAspectAttribute : Attribute
    {
        public int Order { get; set; }
    }
}