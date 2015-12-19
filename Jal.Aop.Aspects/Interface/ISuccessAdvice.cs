using System.Reflection;

namespace Jal.Aop.Aspects.Interface
{
    public interface ISuccessAdvice
    {
        void Handle(object[] arguments, object returnValue, MethodInfo method, object[] parameters, object target);
    }
}
