using System;

namespace Jal.Aop.Aspects.Interface
{
    public interface IAspectLogger
    {
        void OnExit(string classname, string methodname, object @return, string correlationid, string customtemplate, long duration, bool logduration, IAspectSerializer serializer);

        void OnEntry(string classname, string methodname, object[] arguments, string correlationid, string customtemplate, IAspectSerializer serializer);

        void OnException(string classname, string methodname, string correlationid, string customtemplate, Exception ex, IAspectSerializer serializer);
    }
}
