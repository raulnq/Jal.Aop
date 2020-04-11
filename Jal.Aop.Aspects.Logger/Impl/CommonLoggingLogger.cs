using System;
using Common.Logging;

namespace Jal.Aop.Aspects.Logger
{
    public class CommonLoggingLogger : ILogger
    {
        private readonly ILog _log;

        public readonly string OnExceptionTemplate = "[{class}.cs, {method}] Exception.";

        public readonly string OnExceptionTemplateWithRequestId = "[{class}.cs, {method}, {requestid}] Exception.";

        public readonly string OnEntryTemplateAndArguments = "[{class}.cs, {method}] Start Call. Arguments: {arguments}.";

        public readonly string OnEntryTemplateWithRequestIdAndArguments = "[{class}.cs, {method}, {requestid}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplateWithDurationAndReturn = "[{class}.cs, {method}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateWithReturn = "[{class}.cs, {method}] End Call. Return Value: {return}";

        public readonly string OnExitTemplateWithRequestIdAndDurationAndReturn = "[{class}.cs, {method}, {requestid}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExitTemplateWithRequestIdAndReturn = "[{class}.cs, {method}, {requestid}] End Call. Return Value: {return}";

        private readonly ISerializer _serializer;

        public CommonLoggingLogger(ILog log, ISerializer serializer)
        {
            _log = log;

            _serializer = serializer;
        }

        public void OnExit(IJoinPoint joinpoint, Return @return, string requestid, long duration)
        {
            var returnvalue = string.Empty;

            var template = OnExitTemplateWithDurationAndReturn;

            if (!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnExitTemplateWithRequestIdAndDurationAndReturn;
            }

            var value = _serializer.Serialize(@return.Value);

            if (!string.IsNullOrWhiteSpace(value))
            {
                var s = string.Format("{0} = {1}", @return.Type, value);

                returnvalue = string.Format("{0}, {1}", s, returnvalue);
            }

            if (string.IsNullOrWhiteSpace(returnvalue))
            {
                returnvalue = "None";
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{took}", duration.ToString());

            message = message.Replace("{return}", returnvalue);

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnEntry(IJoinPoint joinpoint, Argument[] arguments, string requestid)
        {
            var parameters = string.Empty;

            var template = OnEntryTemplateAndArguments;

            if(!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnEntryTemplateWithRequestIdAndArguments;
            }

            if(arguments!=null && arguments.Length>0)
            {
                foreach (var argument in arguments)
                {
                    var value = _serializer.Serialize(argument.Value);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var s = string.Format("{0} {1} = {2}", argument.Type, argument.Name, value);

                        parameters = string.Format("{0}, {1}", s, parameters);
                    }
                }
            }


            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "None";
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{arguments}", parameters);

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnException(IJoinPoint joinpoint, string requestid, Exception ex)
        {
            var template = OnExceptionTemplate;

            if (!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnExceptionTemplateWithRequestId;
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{requestid}", requestid);

            _log.Error(message, ex);
        }

        public void OnExit(IJoinPoint joinpoint, string requestid, long duration)
        {
            var returnvalue = "None";

            var template = OnExitTemplateWithDurationAndReturn;

            if (!string.IsNullOrEmpty(requestid))
            {
                template = OnExitTemplateWithRequestIdAndDurationAndReturn;
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{took}", duration.ToString());

            message = message.Replace("{return}", returnvalue);

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnExit(IJoinPoint joinpoint, Return @return, string requestid)
        {
            var returnvalue = string.Empty;

            var template = OnExitTemplateWithReturn;

            if (!string.IsNullOrWhiteSpace(requestid))
            {
                template = OnExitTemplateWithRequestIdAndReturn;
            }

            var value = _serializer.Serialize(@return.Value);

            if (!string.IsNullOrWhiteSpace(value))
            {
                var s = string.Format("{0} = {1}", @return.Type, value);

                returnvalue = string.Format("{0}, {1}", s, returnvalue);
            }

            if (string.IsNullOrWhiteSpace(returnvalue))
            {
                returnvalue = "None";
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{return}", returnvalue);

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }

        public void OnExit(IJoinPoint joinpoint, string requestid)
        {
            var returnvalue = "None";

            var template = OnExitTemplateWithReturn;

            if(!string.IsNullOrEmpty(requestid))
            {
                template = OnExitTemplateWithRequestIdAndReturn;
            }

            var message = template.Replace("{class}", joinpoint.TargetType.Name);

            message = message.Replace("{method}", joinpoint.MethodInfo.Name);

            message = message.Replace("{return}", returnvalue);

            message = message.Replace("{requestid}", requestid);

            _log.Debug(message);
        }
    }
}
