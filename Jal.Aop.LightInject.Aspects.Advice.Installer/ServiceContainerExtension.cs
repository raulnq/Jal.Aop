using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jal.Aop.Aspects.Advice;
using Jal.Aop.Aspects.Interface;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Advice.Installer
{
    public static class ServiceContainerExtensions
    {
        public static void RegisterAdvices(this IServiceContainer container, Assembly[] assemblies, ILifetime lifetime=null)
        {
            container.Register<IAdviceFactory, AdviceFactory>(new PerContainerLifetime());

            container.Register<IExceptionAdvice, ExceptionAdvice>(typeof(ExceptionAdvice).FullName, new PerContainerLifetime());

            var exceptionHandlerTypes = GetTypesOf<IExceptionAdvice>(assemblies);

            foreach (var exceptionHandlerType in exceptionHandlerTypes)
            {
                if (lifetime == null)
                {
                    container.Register(typeof(IExceptionAdvice), exceptionHandlerType, exceptionHandlerType.FullName);
                }
                if (lifetime is PerContainerLifetime)
                {
                    container.Register(typeof(IExceptionAdvice), exceptionHandlerType, exceptionHandlerType.FullName, new PerContainerLifetime());
                }
                if (lifetime is PerScopeLifetime)
                {
                    container.Register(typeof(IExceptionAdvice), exceptionHandlerType, exceptionHandlerType.FullName, new PerScopeLifetime());
                }

                if (lifetime is PerRequestLifeTime)
                {
                    container.Register(typeof(IExceptionAdvice), exceptionHandlerType, exceptionHandlerType.FullName, new PerRequestLifeTime());
                }
            }

            var successHandlerTypes = GetTypesOf<ISuccessAdvice>(assemblies);

            foreach (var successHandlerType in successHandlerTypes)
            {
                if (lifetime == null)
                {
                    container.Register(typeof(ISuccessAdvice), successHandlerType, successHandlerType.FullName);
                }
                if (lifetime is PerContainerLifetime)
                {
                    container.Register(typeof(ISuccessAdvice), successHandlerType, successHandlerType.FullName, new PerContainerLifetime());
                }
                if (lifetime is PerScopeLifetime)
                {
                    container.Register(typeof(ISuccessAdvice), successHandlerType, successHandlerType.FullName, new PerScopeLifetime());
                }

                if (lifetime is PerRequestLifeTime)
                {
                    container.Register(typeof(ISuccessAdvice), successHandlerType, successHandlerType.FullName, new PerRequestLifeTime());
                }
            }

            var entryHandlerTypes = GetTypesOf<IEntryAdvice>(assemblies);

            foreach (var entryHandlerType in entryHandlerTypes)
            {
                if (lifetime == null)
                {
                    container.Register(typeof(IEntryAdvice), entryHandlerType, entryHandlerType.FullName);
                }
                if (lifetime is PerContainerLifetime)
                {
                    container.Register(typeof(IEntryAdvice), entryHandlerType, entryHandlerType.FullName, new PerContainerLifetime());
                }
                if (lifetime is PerScopeLifetime)
                {
                    container.Register(typeof(IEntryAdvice), entryHandlerType, entryHandlerType.FullName, new PerScopeLifetime());
                }

                if (lifetime is PerRequestLifeTime)
                {
                    container.Register(typeof(IEntryAdvice), entryHandlerType, entryHandlerType.FullName, new PerRequestLifeTime());
                }
            }
        }

        public static Type[] GetTypesOf<T>(Assembly[] assemblies)
        {
            var type = typeof(T);
            var instances = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var assemblyInstance = (
                    assembly.GetTypes()
                    .Where(t => type.IsAssignableFrom(t))
                    ).ToArray();
                instances.AddRange(assemblyInstance);
            }
            return instances.ToArray();
        }
    }
}