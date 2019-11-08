using System;
using System.Collections.Generic;
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

            var parameters = new List<object>();

            if (currentAttribute.LogArguments)
            {
                var position = 0;

                foreach (var parameter in invocation.Arguments)
                {
                    if (currentAttribute.SkipArguments == null || !currentAttribute.SkipArguments.Contains(position))
                    {
                        parameters.Add(parameter);
                    }
                    position++;
                }
            }

            if (currentAttribute.LogCorrelationId)
            {
                if (Provider != null)
                {
                    CorrelationId = Provider.Provide(invocation.Arguments, invocation.TargetObject, invocation.MethodInfo);
                }
            }

            Log.OnEntry(invocation.TargetType.Name, invocation.MethodInfo.Name, parameters.ToArray(), CorrelationId, currentAttribute.OnEntryMessageTemplate, Serializer);

            StopWatch.Start();
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            StopWatch.Stop();

            var currentAttribute = CurrentAttribute(invocation);

            if(currentAttribute.LogReturnValue)
            {
                Log.OnExit(invocation.TargetType.Name, invocation.MethodInfo.Name, invocation.ReturnValue, CorrelationId, currentAttribute.OnExitMessageTemplate, StopWatch.ElapsedMilliseconds, currentAttribute.LogDuration, Serializer);
            }
            else
            {
                Log.OnExit(invocation.TargetType.Name, invocation.MethodInfo.Name, null, CorrelationId, currentAttribute.OnExitMessageTemplate, StopWatch.ElapsedMilliseconds, currentAttribute.LogDuration, Serializer);
            }
        }

        protected override void OnException(IJoinPoint invocation, Exception ex)
        {
            var currentAttribute = CurrentAttribute(invocation);

            Log.OnException(invocation.TargetType.Name, invocation.MethodInfo.Name, CorrelationId, currentAttribute.OnExceptionMessageTemplate, ex, Serializer);

            throw new Exception("Exception logged by the LogAspect", ex);
        }
    }
}
