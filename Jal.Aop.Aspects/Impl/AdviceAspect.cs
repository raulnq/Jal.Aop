using System;

namespace Jal.Aop.Aspects
{
    public class AdviceAspect : OnMethodBoundaryAspect<AdviceAspectAttribute>
    {
        private IFactory<IAdvice> _factory;

        private IAdvice _advice;

        public AdviceAspect(IFactory<IAdvice> factory)
        {
            HandleException = false;

            _factory = factory;
        }

        protected override void Init(IJoinPoint joinPoint)
        {
            if(CurrentAttribute.Type == null)
            {
                throw new Exception("The Type should not be null");
            }

            if (CurrentAttribute.Type != null && !typeof(IAdvice).IsAssignableFrom(CurrentAttribute.Type))
            {
                throw new Exception("The Type used is not valid");
            }

            _advice = _factory.Create(joinPoint, CurrentAttribute.Type);

            HandleException = CurrentAttribute.HandleException;
        }

        protected override void OnSuccess(IJoinPoint joinPoint)
        {
            _advice.OnSuccess(joinPoint, CurrentAttribute.Context);
        }

        protected override void OnEntry(IJoinPoint joinPoint)
        {
            _advice.OnEntry(joinPoint, CurrentAttribute.Context);
        }

        protected override void OnExit(IJoinPoint joinPoint)
        {
            _advice.OnExit(joinPoint, CurrentAttribute.Context);
        }

        protected override void OnException(IJoinPoint joinPoint, Exception ex)
        {
            _advice.OnException(joinPoint, CurrentAttribute.Context, ex);
        }
    }
}
