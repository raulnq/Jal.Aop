namespace Jal.Aop.Aspects
{
    public interface IExpressionEvaluator
    {
        TOutput Evaluate<TOutput>(IJoinPoint joinPoint, string expression, TOutput errorvalue = default(TOutput));
    }
}
