using Jal.Aop.Aspects;
using Jal.Aop.DispatchProxy;
using Jal.Locator;
using Jal.Locator.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Aop.Apects.Microsoft.Extensions.DependencyInjection.Installer
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLoggerForAop<T>(this IServiceCollection container) where T : class, ILogger
        {
            return container.AddSingleton<ILogger, T>();
        }

        public static IServiceCollection AddAdviceForAop<T>(this IServiceCollection container) where T : class, IAdvice
        {
            return container.AddSingleton<IAdvice, T>();
        }

        public static IServiceCollection AddAop(this IServiceCollection servicecollection, Type[] aspecttypes = null, Action<IServiceCollection> action = null, bool automaticInterception = true)
        {
            servicecollection.AddServiceLocator();

            var aspects = new List<Type>(aspecttypes ?? new Type[] { })
                              {
                                  typeof (LoggerAspect),
                                  typeof (AdviceAspect)
                              }.ToArray();

            servicecollection.AddSingleton<IPointCut, PointCut>();

            servicecollection.AddSingleton<IAspectExecutor>(factory => new AspectExecutor(aspects, factory.GetService<IServiceLocator>(), factory.GetService<IPointCut>()));

            foreach (var type in aspects)
            {
                if (!typeof(IAspect).IsAssignableFrom(type))
                {
                    throw new Exception($"The type {type.FullName} is a not valid IAspect implementation");
                }
                servicecollection.AddTransient(typeof(IAspect), type);
            }

            servicecollection.AddSingleton<IFactory<IAdvice>, Factory<IAdvice>>();

            servicecollection.AddSingleton<IFactory<ILogger>, Factory<ILogger>>();

            servicecollection.AddSingleton<IEvaluator, Evaluator>();

            servicecollection.AddSingleton<IAdvice, Advice>();

            if (action != null)
            {
                action(servicecollection);
            }

            if(automaticInterception)
            {
                var descriptorstoproxy = new List<ServiceDescriptor>();

                foreach (var descriptor in servicecollection)
                {
                    if (descriptor.ServiceType != null && descriptor.ImplementationType != null)
                    {
                        var methods = descriptor.ImplementationType.GetMethods();

                        if (methods.Select(methodInfo => methodInfo.GetCustomAttributes(typeof(AbstractAspectAttribute), true)).Any(attributes => attributes.Length > 0))
                        {
                            descriptorstoproxy.Add(descriptor);
                        }
                    }
                }

                foreach (var descriptor in descriptorstoproxy)
                {
                    servicecollection.Remove(descriptor);

                    var newservicedescriptor = ServiceDescriptor.Describe(descriptor.ImplementationType, descriptor.ImplementationType, descriptor.Lifetime);

                    servicecollection.Add(newservicedescriptor);

                    var proxyservicedescriptor = ServiceDescriptor.Describe(descriptor.ServiceType, x => AopProxyCreator.Create(descriptor.ServiceType, descriptor.ImplementationType, x.GetService(descriptor.ImplementationType), x.GetService<IAspectExecutor>()), descriptor.Lifetime);

                    servicecollection.Add(proxyservicedescriptor);
                }
            }

            return servicecollection;
        }
    }
}
