using System;
using Serilog;

namespace Jal.Aop.Aspects.Logger.Serilog
{
    public class SerilogLogger : ILogger
    {
        public readonly string OnExceptionTemplate = "[{ClassName}, {MethodName}] Exception.";

        public readonly string OnEntryTemplate = "[{ClassName}, {MethodName}] Start Call.";

        public readonly string OnExitTemplate = "[{ClassName}, {MethodName}] End Call. Took {Duration} ms.";

        public readonly string OnExitTemplateNoDuration = "[{ClassName}, {MethodName}] End Call.";

        public readonly string OnExceptionTemplateWithRequestId = "[{ClassName}, {MethodName}, {Id}] Exception.";

        public readonly string OnEntryTemplateWithRequestId = "[{ClassName}, {MethodName}, {Id}] Start Call.";

        public readonly string OnExitTemplateWithRequestId = "[{ClassName}, {MethodName}, {Id}] End Call. Took {Duration} ms.";

        public readonly string OnExitTemplateWithRequestIdNoDuration = "[{ClassName}, {MethodName}, {Id}] End Call.";

        public void OnExit(string classname, string methodname, object @return, string requestid, string customtemplate, long duration, ISerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(requestid))
            {
                var template = duration!=-1 ? OnExitTemplateWithRequestId : OnExitTemplateWithRequestIdNoDuration;

                if(@return!=null)
                {
                    var log = Log.ForContext("Return", @return, true);
                    log.Debug(template, classname, methodname, requestid, duration);
                }
                else
                {
                    Log.Debug(template, classname, methodname, requestid, duration);
                }
            }
            else
            {
                var template = duration != -1 ? OnExitTemplate : OnExitTemplateNoDuration;

                if (@return != null)
                {
                    var log = Log.ForContext("Return", @return, true);
                    log.Debug(template, classname, methodname, duration);
                }
                else
                {
                    Log.Debug(template, classname, methodname, duration);
                }   
            }
        }

        public void OnEntry(string classname, string methodname, object[] arguments, string requestid, string customtemplate, ISerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(requestid))
            {
                if (arguments != null && arguments.Length>0)
                {
                    var log = Log.ForContext("Arguments", arguments, true);
                    log.Debug(OnEntryTemplateWithRequestId, classname, methodname, requestid);
                }
                else
                {
                    Log.Debug(OnEntryTemplateWithRequestId, classname, methodname, requestid);
                }
            }
            else
            {
                if (arguments != null && arguments.Length > 0)
                {
                    var log = Log.ForContext("Arguments", arguments, true);
                    log.Debug(OnEntryTemplate, classname, methodname);
                }
                else
                {
                    Log.Debug(OnEntryTemplate, classname, methodname);
                }
            }
        }

        public void OnException(string classname, string methodname, string requestid, string customtemplate, Exception ex, ISerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(requestid))
            {
                Log.Error(ex, OnExceptionTemplateWithRequestId, classname, methodname, requestid);
            }
            else
            {
                Log.Error(ex, OnExceptionTemplate, classname, methodname, requestid);
            }
        }
    }
}
