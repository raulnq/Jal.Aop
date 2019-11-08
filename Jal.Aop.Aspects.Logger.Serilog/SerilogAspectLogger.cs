using System;
using Serilog;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Logger.Serilog
{
    public class SerilogAspectLogger : IAspectLogger
    {
        public readonly string OnExceptionTemplate = "[{ClassName}, {MethodName}] Exception.";

        public readonly string OnEntryTemplate = "[{ClassName}, {MethodName}] Start Call.";

        public readonly string OnExitTemplate = "[{ClassName}, {MethodName}] End Call. Took {Duration} ms.";

        public readonly string OnExitTemplateNoDuration = "[{ClassName}, {MethodName}] End Call.";

        public readonly string OnExceptionTemplateWithCorrelation = "[{ClassName}, {MethodName}, {Id}] Exception.";

        public readonly string OnEntryTemplateWithCorrelation = "[{ClassName}, {MethodName}, {Id}] Start Call.";

        public readonly string OnExitTemplateWithCorrelation = "[{ClassName}, {MethodName}, {Id}] End Call. Took {Duration} ms.";

        public readonly string OnExitTemplateWithCorrelationNoDuration = "[{ClassName}, {MethodName}, {Id}] End Call.";

        public void OnExit(string classname, string methodname, object @return, string correlationid, string customtemplate, long duration, bool logduration, IAspectSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                var template = logduration ? OnExitTemplateWithCorrelation : OnExitTemplateWithCorrelationNoDuration;

                if(@return!=null)
                {
                    var log = Log.ForContext("Return", @return, true);
                    log.Debug(template, classname, methodname, correlationid, duration);
                }
                else
                {
                    Log.Debug(template, classname, methodname, correlationid, duration);
                }
            }
            else
            {
                var template = logduration ? OnExitTemplate : OnExitTemplateNoDuration;

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

        public void OnEntry(string classname, string methodname, object[] arguments, string correlationid, string customtemplate, IAspectSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                if (arguments != null && arguments.Length>0)
                {
                    var log = Log.ForContext("Arguments", arguments, true);
                    log.Debug(OnEntryTemplateWithCorrelation, classname, methodname, correlationid);
                }
                else
                {
                    Log.Debug(OnEntryTemplateWithCorrelation, classname, methodname, correlationid);
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

        public void OnException(string classname, string methodname, string correlationid, string customtemplate, Exception ex, IAspectSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                Log.Error(ex, OnExceptionTemplateWithCorrelation, classname, methodname, correlationid);
            }
            else
            {
                Log.Error(ex, OnExceptionTemplate, classname, methodname, correlationid);
            }
        }
    }
}
