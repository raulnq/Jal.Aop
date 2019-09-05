using System;
using Serilog;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Logger.Serilog
{
    public class SerilogAspectLogger : IAspectLogger
    {
        public readonly string OnExceptionTemplate = "[{classname}.cs, {methodname}] Exception.";

        public readonly string OnEntryTemplate = "[{classname}.cs, {methodname}] Start Call.";

        public readonly string OnExitTemplate = "[{classname}.cs, {methodname}] End Call. Took {duration} ms.";

        public readonly string OnExceptionTemplateWithCorrelation = "[{classname}.cs, {methodname}, {id}] Exception.";

        public readonly string OnEntryTemplateWithCorrelation = "[{classname}.cs, {methodname}, {id}] Start Call.";

        public readonly string OnExitTemplateWithCorrelation = "[{classname}.cs, {methodname}, {id}] End Call. Took {duration} ms.";

        public void OnExit(string classname, string methodname, object @return, string correlationid, string customtemplate, long duration, IAspectSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                if(@return!=null)
                {
                    var log = Log.ForContext("return", @return, true);
                    log.Information(OnExitTemplateWithCorrelation, classname, methodname, correlationid, duration);
                }
                else
                {
                    Log.Information(OnExitTemplateWithCorrelation, classname, methodname, correlationid, duration);
                }
            }
            else
            {
                if (@return != null)
                {
                    var log = Log.ForContext("return", @return, true);
                    log.Information(OnExitTemplate, classname, methodname, duration);
                }
                else
                {
                    Log.Information(OnExitTemplate, classname, methodname, duration);
                }   
            }
        }

        public void OnEntry(string classname, string methodname, object[] arguments, string correlationid, string customtemplate, IAspectSerializer serializer)
        {
            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                if (arguments != null && arguments.Length>0)
                {
                    var log = Log.ForContext("parameters", arguments, true);
                    log.Information(OnEntryTemplateWithCorrelation, classname, methodname, correlationid);
                }
                else
                {
                    Log.Information(OnEntryTemplateWithCorrelation, classname, methodname, correlationid);
                }
            }
            else
            {
                if (arguments != null && arguments.Length > 0)
                {
                    var log = Log.ForContext("parameters", arguments, true);
                    log.Information(OnEntryTemplate, classname, methodname);
                }
                else
                {
                    Log.Information(OnEntryTemplate, classname, methodname);
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
