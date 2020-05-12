using System;


namespace Jal.Aop.Aspects
{
    public class RetryAspect : OnRetryAspect<RetryAspectAttribute>
    {
        public int Count { get; set; }
        protected override bool CanRetry(IJoinPoint joinPoint, Exception ex)
        {
            var attribute = Get(joinPoint);

            if (Count < attribute.MaxAttempts && (attribute.ExceptionType == null || attribute.ExceptionType == ex.GetType()))
            {
                Count++;

                return true;
            }
            return false;
        }
    }
}
