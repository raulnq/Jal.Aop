using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jal.Aop.Aspects;
using Jal.Aop.Impl;
using Jal.Aop.Interface;
using LightInject;

namespace Jal.Aop.LightInject.Aspect.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterAspects(this IServiceContainer container, Assembly[] assemblies)
        {
            var aspectTypesfromAssemblies = GetTypesOf<IAspect>(assemblies);

            var aspectTypes = new List<Type>(aspectTypesfromAssemblies)
                              {
                                  typeof (LogAspect),
                                  typeof (AroundMethodAspect)
                              }.ToArray();

            container.Register<IPointCut, PointCut>(new PerContainerLifetime());

            container.GetInstance<IPointCut>();

            container.Register<AopProxy>(factory => new AopProxy(aspectTypes, container, factory.GetInstance<IPointCut>()));

            foreach (var type in aspectTypes)
            {
                container.Register(type);
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