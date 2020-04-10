using System;

namespace Jal.Aop.Aspects
{
    public interface ILogger
    {
        void OnExit(string classname, string methodname, object @return, string requestid, string customtemplate, long duration, ISerializer serializer);

        void OnEntry(string classname, string methodname, object[] arguments, string requestid, string customtemplate, ISerializer serializer);

        void OnException(string classname, string methodname, string requestid, string customtemplate, Exception ex, ISerializer serializer);
    }
}
