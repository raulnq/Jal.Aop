using System;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Impl
{
    public class NullAspectLogger : IAspectLogger
    {
        public static NullAspectLogger Instance = new NullAspectLogger();

        public void OnExit(string classname, string methodname, object @return, string correlationid, string customtemplate, long duration, IAspectSerializer serializer)
        {

        }

        public void OnEntry(string classname, string methodname, object[] arguments, string correlationid, string customtemplate, IAspectSerializer serializer)
        {

        }

        public void OnException(string classname, string methodname, string correlationid, string customtemplate, Exception ex, IAspectSerializer serializer)
        {

        }
    }
}
