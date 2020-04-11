using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Jal.Aop.Aspects.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddAop(this IWindsorContainer container, Type[] types = null, Action<IWindsorContainer> action = null, bool automaticInterception = true)
        {
            container.Install(new AspectInstaller(types, action, automaticInterception));
        }

        public static void AddAdviceForAop<T>(this IWindsorContainer container) where T: IAdvice
        {
            container.Register(Component.For<IAdvice>().ImplementedBy<T>().Named(typeof(T).FullName));
        }

        public static void AddLoggerForAop<T>(this IWindsorContainer container) where T : ILogger
        {
            container.Register(Component.For<ILogger>().ImplementedBy<T>().Named(typeof(T).FullName));
        }
    }
}