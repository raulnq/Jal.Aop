using System;
using System.Collections.Generic;
using Jal.Aop.Aspects;
using Jal.Locator;
using Jal.Locator.LightInject;
using LightInject;

namespace Jal.Aop.LightInject.Aspect.Installer
{
    public static class ServiceContainerExtension
    {
        public static void AddLoggerForAop<T>(this IServiceContainer container) where T : ILogger
        {
            container.Register<ILogger, T>(typeof(T).FullName, new PerContainerLifetime());
        }

        public static void AddAdviceForAop<T>(this IServiceContainer container) where T : IAdvice
        {
            container.Register<IAdvice, T>(typeof(T).FullName, new PerContainerLifetime());
        }

        public static void AddAop(this IServiceContainer container, Type[] aspecttypes=null, Action<IServiceContainer> action = null, bool automaticInterception = true)
        {
            container.AddServiceLocator();

            var aspects = new List<Type>(aspecttypes??new Type[] { })
                              {
                                  typeof (LoggerAspect),
                                  typeof (AdviceAspect)
                              }.ToArray();

            container.Register<IPointCut, PointCut>(new PerContainerLifetime());

            container.Register<IAspectExecutor>(factory=> new AspectExecutor(aspects, factory.GetInstance<IServiceLocator>(), factory.GetInstance<IPointCut>()) , new PerContainerLifetime());

            container.Register<AopProxy>();

            foreach (var type in aspects)
            {
                if (!typeof(IAspect).IsAssignableFrom(type))
                {
                    throw new Exception($"The type {type.FullName} is a not valid IAspect implementation");
                }
                container.Register(typeof(IAspect), type, type.FullName);
            }

            container.Register<IFactory<IAdvice>, Factory<IAdvice>>(new PerContainerLifetime());

            container.Register<IFactory<ILogger>, Factory<ILogger>>(new PerContainerLifetime());

            container.Register<IEvaluator, Evaluator>(new PerContainerLifetime());

            container.Register<IAdvice, Advice>(typeof(Advice).FullName, new PerContainerLifetime());

            if(action!=null)
            {
                action(container);
            }

            if (automaticInterception)
            {
                container.RegisterFrom<AutomaticInterceptionCompositionRoot>();
            }
        }
    }
}