using System;
using System.Diagnostics;
using System.Linq;
using Jal.Aop.Aspects.Impl;
using Jal.Aop.Aspects.Interface;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Aspects
{
    public class LogAspect : OnMethodBoundaryAspect<LogAspectAttribute>
    {
        public IAspectLogger Log { get; set; }

        public ICorrelationIdProviderFactory CorrelationIdProviderFactory { get; set; }

        public IAspectSerializerFactory SerializerFactory { get; set; }

        protected Stopwatch StopWatch;

        protected string CorrelationId;

        public readonly string OnExceptionTemplate = "[{class}.cs, {method}] Exception.";

        public readonly string OnEntryTemplate  = "[{class}.cs, {method}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplate = "[{class}.cs, {method}] End Call. Took {took} ms. Return Value: {return}";

        public readonly string OnExceptionTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] Exception.";

        public readonly string OnEntryTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] Start Call. Arguments: {arguments}.";

        public readonly string OnExitTemplateWithCorrelation = "[{class}.cs, {method}, {correlationid}] End Call. Took {took} ms. Return Value: {return}";

        protected IAspectSerializer Serializer;

        protected ICorrelationIdProvider Provider;

        public LogAspect()
        {
            HandleException = false;
        }

        protected override void Initialize(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            if (currentAttribute.CorrelationIdProviderType != null && !typeof(ICorrelationIdProvider).IsAssignableFrom(currentAttribute.CorrelationIdProviderType))
            {
                throw new Exception("The type used in the property SuccessAdviceType is not valid");
            }

            if (currentAttribute.SerializerType != null && !typeof(IAspectSerializer).IsAssignableFrom(currentAttribute.SerializerType))
            {
                throw new Exception("The type used in the property SerializerType is not valid");
            }
            if (SerializerFactory == null)
            {
                SerializerFactory = NullAspectSerializerFactory.Instance;
            }
            if (CorrelationIdProviderFactory == null)
            {
                CorrelationIdProviderFactory = NullCorrelationIdProviderFactory.Instance;
            }
            if (SerializerFactory == null)
            {
                SerializerFactory = NullAspectSerializerFactory.Instance;
            }

            if (Log == null)
            {
                Log = NullAspectLogger.Instance;
            }

            Serializer = SerializerFactory.Create(currentAttribute.SerializerType);

            if (currentAttribute.CorrelationIdProviderType != null)
            {
                Provider = CorrelationIdProviderFactory.Create(currentAttribute.CorrelationIdProviderType);
            }
            StopWatch = new Stopwatch();

            if (currentAttribute.LogException)
            {
                HandleException = true;
            }
        }

        protected override void OnEntry(IJoinPoint invocation)
        {
            var currentAttribute = CurrentAttribute(invocation);

            var parameters = string.Empty;

            if (currentAttribute.LogArguments)
            {
                var position = 0;

                foreach (var parameter in invocation.Arguments)
                {
                    if (currentAttribute.SkipArguments == null || !currentAttribute.SkipArguments.Contains(position))
                    {
                        if (Serializer != null)
                        {
                            var value = Serializer.Serialize(parameter, position);

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var s = string.Format("{0} = {1}", parameter.GetType().Name, value);

                                parameters = string.Format("{0}, {1}", s, parameters);
                            }
                        }
                    }
                    position++;
                }
            }
            if (string.IsNullOrWhiteSpace(parameters))
            {
                parameters = "None";
            }

            var template = OnEntryTemplate;

            if (currentAttribute.LogCorrelationId)
            {
                template = OnEntryTemplateWithCorrelation;

                if (Provider != null)
                {
                    CorrelationId = Provider.Provide(invocation.Arguments, invocation.TargetObject, invocation.MethodInfo);
                }
            }

            if(!string.IsNullOrWhiteSpace(currentAttribute.OnEntryMessageTemplate))
            {
                template = currentAttribute.OnEntryMessageTemplate;
            }

            var message = template.Replace("{class}", invocation.TargetType.Name);

            message = message.Replace("{method}", invocation.MethodInfo.Name);

            message = message.Replace("{arguments}", parameters);

            if (currentAttribute.LogCorrelationId)
            {
                message = message.Replace("{correlationid}", CorrelationId);
            }

            Log.Info(message);

            StopWatch.Start();
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            StopWatch.Stop();

            var currentAttribute = CurrentAttribute(invocation);

            var returnvalue = string.Empty;

            if (currentAttribute.LogReturnValue && invocation.ReturnValue!=null)
            {
                if (Serializer != null)
                {
                    var value = Serializer.Serialize(invocation.ReturnValue, 0);

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var s = string.Format("{0} = {1}", invocation.ReturnValue.GetType().Name, value);

                        returnvalue = string.Format("{0}, {1}", s, returnvalue);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(returnvalue))
            {
                returnvalue = "None";
            }

            var template = OnExitTemplate;


            if (currentAttribute.LogCorrelationId)
            {
                template = OnEntryTemplateWithCorrelation;
            }

            if (!string.IsNullOrWhiteSpace(currentAttribute.OnExitMessageTemplate))
            {
                template = currentAttribute.OnExitMessageTemplate;
            }

            var message = template.Replace("{class}", invocation.TargetType.Name);

            message = message.Replace("{method}", invocation.MethodInfo.Name);

            message = message.Replace("{took}", StopWatch.ElapsedMilliseconds.ToString());

            message = message.Replace("{return}", returnvalue);

            if (currentAttribute.LogCorrelationId)
            {
                message = message.Replace("{correlationid}", CorrelationId);
            }

            Log.Info(message);
        }

        protected override void OnException(IJoinPoint invocation, Exception ex)
        {
            var currentAttribute = CurrentAttribute(invocation);

            var template =OnExceptionTemplate;

            if (currentAttribute.LogCorrelationId)
            {
                template =OnExceptionTemplateWithCorrelation;
            }

            if (!string.IsNullOrWhiteSpace(currentAttribute.OnExceptionMessageTemplate))
            {
                template = currentAttribute.OnExceptionMessageTemplate;
            }

            var message = template.Replace("{class}", invocation.TargetType.Name);

            message = message.Replace("{method}", invocation.MethodInfo.Name);

            if (currentAttribute.LogCorrelationId)
            {
                message = message.Replace("{correlationid}", CorrelationId);
            }

            Log.Error(message, ex);

            throw new Exception("Exception logged by the LogAspect", ex);
        }
    }
}
