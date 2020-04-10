using System;
using Common.Logging;

namespace Jal.Aop.Aspects.Logger
{
    public class CommonLoggingLogger : ILogger
    {
        private readonly ILog _log;

        public readonly string OnExceptionTemplate = "[{class}.cs, {method}] Exception.";

        public readonly string OnEntryTemplate = "[{class}.cs, {method}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplate = "[{class}.cs, {method}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateNoDuration = "[{class}.cs, {method}] End Call. Return Value: {return}";

        public readonly string OnExceptionTemplateWithRequestId = "[{class}.cs, {method}, {requestid}] Exception.";

        public readonly string OnEntryTemplateWithRequestId = "[{class}.cs, {method}, {requestid}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplateWithRequestId = "[{class}.cs, {method}, {requestid}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateWithRequestIdNoDuration = "[{class}.cs, {method}, {requestid}] End Call. Return Value: {return}";

        public CommonLoggingLogger(ILog log)
        {
            _log = log;
        }

        public void OnExit(string classname, string methodname, object @return, string requestid, string customtemplate, long duration, ISerializer serializer)
        {
            var returnvalue = string.Empty;

            var template = OnExitTemplate;

            if (!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnExitTemplateWithRequestId;
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

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnEntry(string classname, string methodname, object[] arguments, string requestid, string customtemplate, ISerializer serializer)
        {
            var position = 0;

            var parameters = string.Empty;

            var template = OnEntryTemplate;

            if(!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnEntryTemplateWithRequestId;
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

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnException(string classname, string methodname, string requestid, string customtemplate, Exception ex, ISerializer serializer)
        {
            var template = OnExceptionTemplate;

            if (!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnExceptionTemplateWithRequestId;
            }

            if (!string.IsNullOrWhiteSpace(customtemplate))
            {
                template = customtemplate;
            }

            var message = template.Replace("{class}", classname);

            message = message.Replace("{method}", methodname);

            message = message.Replace("{requestid}", requestid);

            _log.Error(message, ex);
        }
    }
}
