using System.Reflection;

namespace Jal.Aop.Aspects.Interface
{
    public interface IEntryAdvice
    {
        void Handle(object[] arguments,  MethodInfo method, object[] parameters, object target);
    }
}
