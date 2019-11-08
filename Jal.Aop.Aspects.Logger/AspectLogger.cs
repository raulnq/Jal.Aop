using System;
using Common.Logging;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Logger
{
    public class AspectLogger : IAspectLogger
    {
        private readonly ILog _log;

        public readonly string OnExceptionTemplate = "[{class}.cs, {method}] Exception.";

        public readonly string OnEntryTemplate = "[{class}.cs, {method}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplate = "[{class}.cs, {method}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateNoDuration = "[{class}.cs, {method}] End Call. Return Value: {return}";

        public readonly string OnExceptionTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] Exception.";

        public readonly string OnEntryTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateWithCorrelationNoDuration = "[{class}.cs, {method}, {correlationid}] End Call. Return Value: {return}";

        public AspectLogger(ILog log)
        {
            _log = log;
        }

        public void OnExit(string classname, string methodname, object @return, string correlationid, string customtemplate, long duration, bool logduration, IAspectSerializer serializer)
        {
            var returnvalue = string.Empty;

            var template = OnExitTemplate;

            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                template = OnExitTemplateWithCorrelation;
            }

            if (!string.IsNullOrWhiteSpace(customtemplate))
            {
                template = customtemplate;
            }

            if (@return!=null && serializer != null)
            {
                var value = serializer.Serialize(@return);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var s = string.Format("{0} = {1}", @return.GetType().Name, value);

                    returnvalue = string.Format("{0}, {1}", s, returnvalue);
                }
            }

            if (string.IsNullOrWhiteSpace(returnvalue))
            {
                returnvalue = "None";
            }

            var message = template.Replace("{class}", classname);

            message = message.Replace("{method}", methodname);

            message = message.Replace("{took}", duration.ToString());

            message = message.Replace("{return}", returnvalue);

            message = message.Replace("{correlationid}", correlationid);

            _log.Debug(message);
        }

        public void OnEntry(string classname, string methodname, object[] arguments, string correlationid, string customtemplate, IAspectSerializer serializer)
        {
            var position = 0;

            var parameters = string.Empty;

            var template = OnEntryTemplate;

            if(!string.IsNullOrWhiteSpace(correlationid))
            {
                template = OnEntryTemplateWithCorrelation;
            }

            if (!string.IsNullOrWhiteSpace(customtemplate))
            {
                template = customtemplate;
            }

            if(arguments!=null && serializer!=null)
            {
                foreach (var parameter in arguments)
                {
                    var value = serializer.Serialize(parameter);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var t = parameter == null ? string.Empty : parameter.GetType().Name;

                        var s = string.Format("{0} = {1}", t, value);

                        parameters = string.Format("{0}, {1}", s, parameters);
                    }
                    position++;
                }
            }


            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "None";
            }

            var message = template.Replace("{class}", classname);

            message = message.Replace("{method}", methodname);

            message = message.Replace("{arguments}", parameters);

            message = message.Replace("{correlationid}", correlationid);

            _log.Debug(message);
        }

        public void OnException(string classname, string methodname, string correlationid, string customtemplate, Exception ex, IAspectSerializer serializer)
        {
            var template = OnExceptionTemplate;

            if (!string.IsNullOrWhiteSpace(correlationid))
            {
                template = OnExceptionTemplateWithCorrelation;
            }

            if (!string.IsNullOrWhiteSpace(customtemplate))
            {
                template = customtemplate;
            }

            var message = template.Replace("{class}", classname);

            message = message.Replace("{method}", methodname);

            message = message.Replace("{correlationid}", correlationid);

            _log.Error(message, ex);
        }
    }
}
