using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jal.Aop.Aspects.Interface
{
    public interface ICorrelationIdProvider
    {
        string Provide(object[] arguments, object target, MethodInfo method);
    }
}
