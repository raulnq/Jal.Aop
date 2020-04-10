namespace Jal.Aop
{
    public interface IAspectExecutor
    {
        void Execute(IJoinPoint joinPoint);
    }
}
