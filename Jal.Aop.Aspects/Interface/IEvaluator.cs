namespace Jal.Aop.Aspects
{
    public interface IEvaluator
    {
        TOutput Evaluate<TOutput>(IJoinPoint joinPoint, string expression, TOutput errorvalue = default(TOutput));
    }
}
