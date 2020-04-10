namespace Jal.Aop
{
    public interface IAspect
    {
        void SetNext(IAspect aspect);

        IAspect GetNext();

        void Apply(IJoinPoint joinPoint);

        int GetOrder(IJoinPoint joinPoint);
    }
}
