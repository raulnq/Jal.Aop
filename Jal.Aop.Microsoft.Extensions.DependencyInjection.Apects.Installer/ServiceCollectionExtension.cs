using Jal.Aop.Aspects;
using Jal.Aop.DispatchProxy;
using Jal.Locator;
using Jal.Locator.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Aop.Microsoft.Extensions.DependencyInjection.Apects.Installer
{
    public class AopAspectsBuilder : IAopAspectsBuilder
    {
        private readonly IServiceCollection _container;

        public List<Type> Types { get; }

        public AopAspectsBuilder(IServiceCollection container, List<Type> types)
        {
            _container = container;

            Types = types;
        }

        public IAopAspectsBuilder AddAdvice<TImplementation>() where TImplementation : class, IAdvice
        {
            _container.AddSingleton<IAdvice, TImplementation>();

            return this;
        }

        public IAopAspectsBuilder AddLogger<TImplementation>() where TImplementation : class, ILogger
        {
            _container.AddSingleton<ILogger, TImplementation>();

            return this;
        }

        public IAopAspectsBuilder AddAspect<TImplementation>() where TImplementation : class, IAspect
        {
            var type = typeof(TImplementation);

            Types.Add(type);

            _container.AddTransient(typeof(IAspect), type);

            return this;
        }
    }

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAop(this IServiceCollection servicecollection, Action<IAopAspectsBuilder> action = null, bool automaticInterception = true)
        {
            servicecollection.AddServiceLocator();

            var types = new List<Type>();

            var builder = new AopAspectsBuilder(servicecollection, types);

            builder.AddAspect<LoggerAspect>();

            builder.AddAspect<AdviceAspect>();

            if (action != null)
            {
                action(builder);
            }

            servicecollection.AddSingleton<IPointCut, PointCut>();

            servicecollection.AddSingleton<IAspectExecutor>(factory => new AspectExecutor(builder.Types.ToArray(), factory.GetService<IServiceLocator>(), factory.GetService<IPointCut>())); ;

            servicecollection.AddSingleton<IFactory<IAdvice>, Factory<IAdvice>>();

            servicecollection.AddSingleton<IFactory<ILogger>, Factory<ILogger>>();

            servicecollection.AddSingleton<IEvaluator, Evaluator>();

            servicecollection.AddSingleton<IAdvice, Advice>();

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
