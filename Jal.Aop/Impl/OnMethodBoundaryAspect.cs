using System;
using Jal.Aop.Interface;

namespace Jal.Aop.Impl
{
    public abstract class OnMethodBoundaryAspect<T> : AbstractAspect<T> where T : AbstractAspectAttribute
    {
        protected bool HandleException { get; set; }

        public override void Apply(IJoinPoint joinPoint)
        {
            Initialize(joinPoint);

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
