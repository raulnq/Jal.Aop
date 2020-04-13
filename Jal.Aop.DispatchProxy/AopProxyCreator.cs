using System;
using System.Linq;
using System.Reflection;

namespace Jal.Aop.DispatchProxy
{
    public class AopProxyCreator
    {
        public static TService Create<TService, TImplementation>(TService target, IAspectExecutor executor) where TImplementation : TService
        {
            object proxy = System.Reflection.DispatchProxy.Create<TService, AopProxy<TService, TImplementation>>();

            ((AopProxy<TService, TImplementation>)proxy).Init(target, executor);

            return (TService)proxy;
        }

        public static object Create(Type servicetype, Type implementationtype, object target, IAspectExecutor executor)
        {
            var createMethod = typeof(AopProxyCreator)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(info => info.IsGenericMethod && info.Name=="Create");

            var generic = createMethod.MakeGenericMethod(servicetype, implementationtype);

            return generic.Invoke(null, new[] { target, executor });
        }
    }
}
