using System;
using Jal.Aop.Interface;

namespace Jal.Aop.Impl
{
    public abstract class OnRetryAspect<T> : OnMethodBoundaryAspect<T> where T : AbstractAspectAttribute
    {
        public override void Apply(IJoinPoint joinPoint)
        {
            Initialize(joinPoint);

            RetryableCode(joinPoint);
        }

        private void RetryableCode(IJoinPoint joinPoint)
        {
            OnEntry(joinPoint);
            try
            {
                if (Continue(joinPoint))
                {
                    if (NextAspect == null)
                    {
                        joinPoint.Proceed();
                    }
                    else
                    {
                        NextAspect.Apply(joinPoint);
                    }
                    OnSuccess(joinPoint);
                }
            }
            catch (Exception ex)
            {
                if (CanTryAgain(joinPoint, ex))
                {
                    RetryableCode(joinPoint);
                }
                else
                {
                    if (HandleException)
                    {
                        OnException(joinPoint, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            finally
            {
                OnExit(joinPoint);
            }
        }

        protected virtual bool CanTryAgain(IJoinPoint joinPoint, Exception ex)
        {
            return false;
        }
    }
}
