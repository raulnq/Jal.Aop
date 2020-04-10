using System;

namespace Jal.Aop
{
    public abstract class OnRetryAspect<T> : OnMethodBoundaryAspect<T> where T : AbstractAspectAttribute
    {
        public override void Apply(IJoinPoint joinPoint)
        {
            Init(joinPoint);

            Retry(joinPoint);
        }

        private void Retry(IJoinPoint joinPoint)
        {
            OnEntry(joinPoint);

            try
            {
                if (Continue(joinPoint))
                {
                    if (GetNext() == null)
                    {
                        joinPoint.Proceed();
                    }
                    else
                    {
                        GetNext().Apply(joinPoint);
                    }
                    OnSuccess(joinPoint);
                }
            }
            catch (Exception ex)
            {
                if (CanRetry(joinPoint, ex))
                {
                    Retry(joinPoint);
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

        protected virtual bool CanRetry(IJoinPoint joinPoint, Exception ex)
        {
            return false;
        }
    }
}
