namespace Cignium.Framework.Infrastructure.Aop.Aspects.Interface
{
    public interface IRollbackEvaluator
    {
        bool Evaluate(object result);
    }
}
