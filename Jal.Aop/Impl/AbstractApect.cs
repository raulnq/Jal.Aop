using Jal.Aop.Interface;

namespace Jal.Aop.Impl
{
    public abstract class AbstractAspect<T> : IAspect where T : AbstractAspectAttribute
    {
        protected T CurrentAttribute(IJoinPoint joinPoint)
        {
            var attributes = joinPoint.MethodInfo.GetCustomAttributes(typeof(T), true);

            return attributes.Length> 0 ? attributes[0] as T : null;
        }

        protected virtual void Initialize(IJoinPoint joinPoint)
        {

        }

        public IAspect NextAspect { get; set; }

        public virtual void Apply(IJoinPoint joinPoint)
        {
            
        }

        public int GetOrder(IJoinPoint joinPoint)
        {
            var current = CurrentAttribute(joinPoint);

            return current.Order == 0 ? int.MaxValue : current.Order;
        }
    }
}
