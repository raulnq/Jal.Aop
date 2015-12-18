namespace Jal.Aop.Interface
{
    public interface IAspect
    {
        IAspect NextAspect { get; set; }

        void Apply(IJoinPoint joinPoint);

        int GetOrder(IJoinPoint joinPoint);
    }
}
