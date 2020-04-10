using System;

namespace Jal.Aop
{
    public abstract class OnMethodBoundaryAspect<T> : AbstractAspect<T> where T : AbstractAspectAttribute
    {
        protected bool HandleException;

        protected T CurrentAttribute;

        public override void Apply(IJoinPoint joinPoint)
        {
            CurrentAttribute = Get(joinPoint);

            Init(joinPoint);

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
                if (HandleException)
                {
                    OnException(joinPoint, ex);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                OnExit(joinPoint);
            }
        }

        protected virtual bool Continue(IJoinPoint joinPoint)
        {
            return true;
        }

        protected virtual void OnEntry(IJoinPoint joinPoint)
        {

        }

        protected virtual void OnSuccess(IJoinPoint joinPoint)
        {

        }

        protected virtual void OnExit(IJoinPoint joinPoint)
        {

        }

        protected virtual void OnException(IJoinPoint joinPoint, Exception ex)
        {

        }
    }
}
