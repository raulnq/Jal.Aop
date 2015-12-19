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

        public IAspectSerializerFactory SerializerFactory { get; set; }

        protected Stopwatch StopWatch;

        public readonly string OnEntryTemplate  = "[{0}.cs, {1}] Start Call. Arguments: {2}.";

        public readonly string OnExitTemplate = "[{0}.cs, {1}] End Call. Took {3} ms. Return Value: {2}";

        protected IAspectSerializer Serializer;

        public LogAspect()
        {
            HandleException = false;
        }

        protected override void Initialize(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            if (currentAttribute.SerializerType != null && !typeof(IAspectSerializer).IsAssignableFrom(currentAttribute.SerializerType))
            {
                throw new Exception("The type used in the property SerializerType is not valid");
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
            var message = string.Format(OnEntryTemplate, invocation.TargetType.Name, invocation.MethodInfo.Name, parameters);

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

            var message = string.Format(OnExitTemplate, invocation.TargetType.Name, invocation.MethodInfo.Name,returnvalue,  StopWatch.ElapsedMilliseconds);

            Log.Info(message);
        }

        protected override void OnException(IJoinPoint invocation, Exception ex)
        {
            Log.Error(ex);

            throw new Exception("Exception logged by the LogAspect", ex);
        }
    }
}
