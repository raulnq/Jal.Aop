using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Jal.Aop.Aspects
{
    public class LoggerAspect : OnMethodBoundaryAspect<LoggerAspectAttribute>
    {
        private ILogger _logger;

        private IFactory<ILogger> _loggerFactory;

        private IEvaluator _evaluator;

        public LoggerAspect(IEvaluator evaluator, IFactory<ILogger> loggerFactory)
        {

            HandleException = false;

            _loggerFactory = loggerFactory;

            _evaluator = evaluator;
        }

        protected Stopwatch _stopWatch;

        protected string _requestId;

        protected override void Init(IJoinPoint joinPoint)
        {
            if(!string.IsNullOrEmpty(CurrentAttribute.Expression))
            {
                _requestId = _evaluator.Evaluate(joinPoint, CurrentAttribute.Expression, string.Empty);
            }

            if (CurrentAttribute.Type == null)
            {
                throw new Exception("The Type should not be null");
            }

            if (CurrentAttribute.Type != null && !typeof(ILogger).IsAssignableFrom(CurrentAttribute.Type))
            {
                throw new Exception("The type used in the property Type is not valid");
            }

            _logger = _loggerFactory.Create(joinPoint, CurrentAttribute.Type);

            _stopWatch = new Stopwatch();

            if (CurrentAttribute.LogException)
            {
                HandleException = true;
            }
        }

        protected override void OnEntry(IJoinPoint joinPoint)
        {
            var arguments = new List<Argument>();

            if (CurrentAttribute.LogArguments.Length>0)
            {
                var parameters = joinPoint.MethodInfo.GetParameters();

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];

                    if (CurrentAttribute.LogArguments.Any(x=>x== parameter.Name))
                    {
                        arguments.Add(new Argument() { Name = parameter.Name, Type= parameter.ParameterType.Name, Value = joinPoint.Arguments[i] });
                    }
                }
            }

            _logger.OnEntry(joinPoint, arguments.ToArray(), _requestId);

            _stopWatch.Start();
        }

        protected override void OnExit(IJoinPoint joinPoint)
        {
            _stopWatch.Stop();

            var duration = _stopWatch.ElapsedMilliseconds;

            if (CurrentAttribute.LogReturn)
            {
                if(CurrentAttribute.LogDuration)
                {
                    _logger.OnExit(joinPoint, new Return() { Type = joinPoint.MethodInfo.ReturnType?.Name, Value = joinPoint.Return }, _requestId, duration);
                }
                else
                {
                    _logger.OnExit(joinPoint, new Return() { Type = joinPoint.MethodInfo.ReturnType?.Name, Value = joinPoint.Return }, _requestId);
                }
            }
            else
            {
                if (CurrentAttribute.LogDuration)
                {
                    _logger.OnExit(joinPoint, _requestId, duration);
                }
                else
                {
                    _logger.OnExit(joinPoint, _requestId);
                }
            } 
        }

        protected override void OnException(IJoinPoint joinPoint, Exception ex)
        {
            _logger.OnException(joinPoint, _requestId, ex);

            throw new Exception("Exception logged by the LogAspect", ex);
        }
    }
}
