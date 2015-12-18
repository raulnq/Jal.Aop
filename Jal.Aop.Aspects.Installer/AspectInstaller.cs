using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.Impl;
using Jal.Aop.Installer;
using Jal.Aop.Interface;

namespace Jal.Aop.Aspects.Installer
{
    public class AspectInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var assemblies = AssemblyFinder.Impl.AssemblyFinder.Current.GetAssemblies("Aspect");

            var aspectTypesfromAssemblies = AssemblyFinder.Impl.AssemblyFinder.Current.GetTypesOf<IAspect>(assemblies);

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
    }
}