using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Jal.Aop.Aspects
{
    public class LoggerAspect : OnMethodBoundaryAspect<LoggerAspectAttribute>
    {
        private ILogger _logger;

        private IFactory<IRequestIdProvider> _requestIdProviderFactory;

        private IFactory<ISerializer> _serializerFactory;

        private IFactory<ILogger> _loggerFactory;

        public LoggerAspect(IFactory<ISerializer> serializerFactory, IFactory<IRequestIdProvider> requestIdProviderFactory, IFactory<ILogger> loggerFactory)
        {
            _serializerFactory = serializerFactory;

            HandleException = false;

            _requestIdProviderFactory = requestIdProviderFactory;

            _loggerFactory = loggerFactory;
        }

        protected Stopwatch _stopWatch;

        protected string _requestId;

        protected ISerializer _serializer;

        protected IRequestIdProvider _provider;

        protected override void Init(IJoinPoint joinPoint)
        {
            if (CurrentAttribute.RequestIdProviderType != null && !typeof(IRequestIdProvider).IsAssignableFrom(CurrentAttribute.RequestIdProviderType))
            {
                throw new Exception("The type used in the property RequestIdProvider is not valid");
            }
            
            if(CurrentAttribute.RequestIdProviderType!=null)
            {
                _provider = _requestIdProviderFactory.Create(joinPoint, CurrentAttribute.RequestIdProviderType);

                _requestId = _provider.Provide(joinPoint.Arguments, joinPoint.TargetObject, joinPoint.MethodInfo);
            }

            if (CurrentAttribute.SerializerType != null && !typeof(ISerializer).IsAssignableFrom(CurrentAttribute.SerializerType))
            {
                throw new Exception("The type used in the property SerializerType is not valid");
            }

            if (CurrentAttribute.SerializerType != null)
            {
                _serializer = _serializerFactory.Create(joinPoint, CurrentAttribute.SerializerType);
            }

            if (CurrentAttribute.LoggerType == null)
            {
                throw new Exception("The LoggerType should not be null");
            }

            if (CurrentAttribute.LoggerType != null && !typeof(ILogger).IsAssignableFrom(CurrentAttribute.LoggerType))
            {
                throw new Exception("The type used in the property LoggerType is not valid");
            }

            _logger = _loggerFactory.Create(joinPoint, CurrentAttribute.LoggerType);

            _stopWatch = new Stopwatch();

            if (CurrentAttribute.LogException)
            {
                HandleException = true;
            }
        }

        protected override void OnEntry(IJoinPoint joinPoint)
        {
            var parameters = new List<object>();

            if (CurrentAttribute.LogArguments)
            {
                var position = 0;

                foreach (var parameter in joinPoint.Arguments)
                {
                    if (CurrentAttribute.SkipArguments == null || !CurrentAttribute.SkipArguments.Contains(position))
                    {
                        parameters.Add(parameter);
                    }
                    position++;
                }
            }

            _logger.OnEntry(joinPoint.TargetType.Name, joinPoint.MethodInfo.Name, parameters.ToArray(), _requestId, CurrentAttribute.OnEntryMessageTemplate, _serializer);

            _stopWatch.Start();
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            _stopWatch.Stop();

            var duration = _stopWatch.ElapsedMilliseconds;

            var returnvalue = invocation.Return;

            if (!CurrentAttribute.LogReturn)
            {
                returnvalue = null;
            }

            if (!CurrentAttribute.LogDuration)
            {
                duration = -1;
            }

            _logger.OnExit(invocation.TargetType.Name, invocation.MethodInfo.Name, returnvalue, _requestId, CurrentAttribute.OnExitMessageTemplate, duration, _serializer);
        }

        protected override void OnException(IJoinPoint invocation, Exception ex)
        {
            _logger.OnException(invocation.TargetType.Name, invocation.MethodInfo.Name, _requestId, CurrentAttribute.OnExceptionMessageTemplate, ex, _serializer);

            throw new Exception("Exception logged by the LogAspect", ex);
        }
    }
}
