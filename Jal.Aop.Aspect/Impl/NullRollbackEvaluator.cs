using Cignium.Framework.Infrastructure.Aop.Aspects.Interface;

namespace Cignium.Framework.Infrastructure.Aop.Aspects.Impl
{
    public class NullRollbackEvaluator : IRollbackEvaluator
    {
        public bool Evaluate(object result)
        {
            return false;
        }
    }
}
