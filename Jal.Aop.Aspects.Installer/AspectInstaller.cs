using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.CastleWindsor;
using Jal.Aop.Impl;
using Jal.Aop.Interface;

namespace Jal.Aop.Aspects.Installer
{
    public class AspectInstaller : IWindsorInstaller
    {
        private readonly Assembly[] _aspectSourceAssemblies;

        public AspectInstaller(Assembly[] aspectSourceAssemblies)
        {
            _aspectSourceAssemblies = aspectSourceAssemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = _aspectSourceAssemblies;

            var aspectTypesfromAssemblies = GetTypesOf<IAspect>(assemblies);

            var aspectTypes = new List<Type>(aspectTypesfromAssemblies)
                              {
                                  typeof (LogAspect),
                                  typeof (AroundMethodAspect)
                              }.ToArray();

            container.Register(Component.For(typeof(AopProxy)).DependsOn(new { types = aspectTypes, container }).LifestyleTransient());

            container.Register(Component.For<IPointCut>().ImplementedBy<PointCut>());

            foreach (var type in aspectTypes)
            {
                container.Register(Component.For(type).LifestyleTransient());
            }
        }

        public Type[] GetTypesOf<T>(Assembly[] assemblies)
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