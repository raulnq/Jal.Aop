namespace Jal.Aop
{
    public abstract class AbstractAspect<T> : IAspect where T : AbstractAspectAttribute
    {
        protected T Get(IJoinPoint joinPoint)
        {
            var attributes = joinPoint.MethodInfo.GetCustomAttributes(typeof(T), true);

            return attributes.Length> 0 ? attributes[0] as T : default(T);
        }

        protected virtual void Init(IJoinPoint joinPoint)
        {

        }

        public IAspect GetNext()
        {
            return _next;
        }

        public void SetNext(IAspect aspect)
        {
            _next = aspect;
        }

        private IAspect _next;

        public virtual void Apply(IJoinPoint joinPoint)
        {
            
        }

        public int GetOrder(IJoinPoint joinPoint)
        {
            var current = Get(joinPoint);

            return current.Order == 0 ? int.MaxValue : current.Order;
        }
    }
}
