using System.Reflection;

namespace Jal.Aop.Aspects
{
    public interface IRequestIdProvider
    {
        string Provide(object[] arguments, object target, MethodInfo method);
    }
}
