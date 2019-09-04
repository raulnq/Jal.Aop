using System;
using Jal.Aop.Aspects.Impl;
using Jal.Aop.Aspects.Interface;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Aspects
{
    public class AroundMethodAspect : OnMethodBoundaryAspect<AroundMethodAspectAttribute>
    {
        public IAdviceFactory AdviceFactory { get; set; }

        public AroundMethodAspect()
        {
            HandleException = false;
        }

        protected override void Initialize(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            if (currentAttribute.SuccessAdviceType != null && !typeof(ISuccessAdvice).IsAssignableFrom(currentAttribute.SuccessAdviceType))
            {
                throw new Exception("The type used in the property SuccessAdviceType is not valid");
            }
            if (currentAttribute.ExceptionAdviceType != null && !typeof(IExceptionAdvice).IsAssignableFrom(currentAttribute.ExceptionAdviceType))
            {
                throw new Exception("The type used in the property ExceptionAdviceType is not valid");
            }
            if (currentAttribute.EntryAdviceType != null && !typeof(IEntryAdvice).IsAssignableFrom(currentAttribute.EntryAdviceType))
            {
                throw new Exception("The type used in the property EntryAdviceType is not valid");
            }
            if (currentAttribute.ExitAdviceType != null && !typeof(IExitAdvice).IsAssignableFrom(currentAttribute.ExitAdviceType))
            {
                throw new Exception("The type used in the property EntryAdviceType is not valid");
            }
            if (currentAttribute.ExceptionAdviceType != null )
            {
                HandleException = true;
            }
            if (AdviceFactory == null)
            {
                AdviceFactory = NullAdviceFactory.Instance;
            }
        }

        protected override void OnSuccess(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            var handler = AdviceFactory.CreateSuccessAdvice(joinPoint.MethodInfo, currentAttribute.SuccessAdviceType);

            if (handler != null)
            {
                handler.Handle(joinPoint.Arguments, joinPoint.ReturnValue, joinPoint.MethodInfo, currentAttribute.Context, joinPoint.TargetObject);
            }
        }

        protected override void OnEntry(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);
           
            var handler = AdviceFactory.CreateEntryAdvice(joinPoint.MethodInfo, currentAttribute.EntryAdviceType);

            if (handler != null)
            {
                handler.Handle(joinPoint.Arguments, joinPoint.MethodInfo, currentAttribute.Context, joinPoint.TargetObject);
            }
        }

        protected override void OnExit(IJoinPoint joinPoint)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            var handler = AdviceFactory.CreateExitAdvice(joinPoint.MethodInfo, currentAttribute.ExitAdviceType);

            if (handler != null)
            {
                handler.Handle(joinPoint.Arguments,joinPoint.ReturnValue, joinPoint.MethodInfo, currentAttribute.Context, joinPoint.TargetObject);
            }
        }

        protected override void OnException(IJoinPoint joinPoint, Exception ex)
        {
            var currentAttribute = CurrentAttribute(joinPoint);

            var handler = AdviceFactory.CreateExceptionAdvice(ex, joinPoint.MethodInfo, currentAttribute.ExceptionAdviceType);

            if (handler != null)
            {
                joinPoint.ReturnValue = handler.Handle(joinPoint.Arguments, ex, joinPoint.MethodInfo, currentAttribute.Context, joinPoint.TargetObject);
            }
        }
    }
}
