using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.Aspects.Interface;

namespace Jal.Aop.Aspects.Advice.Installer
{
    public class AspectAdviceInstaller : IWindsorInstaller
    {
        private readonly LifestyleType _lifestyleType;

        public AspectAdviceInstaller(LifestyleType lifestyleType = LifestyleType.Singleton)
        {
            _lifestyleType = lifestyleType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IAdviceFactory>().ImplementedBy<AdviceFactory>());

            container.Register(Component.For<IExceptionAdvice>().ImplementedBy<ExceptionAdvice>().Named(typeof(ExceptionAdvice).FullName));

            var assemblies = AssemblyFinder.Impl.AssemblyFinder.Current.GetAssemblies("Advice");

            var exceptionHandlerTypes = AssemblyFinder.Impl.AssemblyFinder.Current.GetTypesOf<IExceptionAdvice>(assemblies);

            foreach (var exceptionHandlerType in exceptionHandlerTypes)
            {
                if (_lifestyleType == LifestyleType.Singleton)
                {
                    container.Register(Component.For<IExceptionAdvice>().ImplementedBy(exceptionHandlerType).Named(exceptionHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Scoped)
                {
                    container.Register(Component.For<IExceptionAdvice>().ImplementedBy(exceptionHandlerType).LifestyleScoped().Named(exceptionHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Transient)
                {
                    container.Register(Component.For<IExceptionAdvice>().ImplementedBy(exceptionHandlerType).LifestyleTransient().Named(exceptionHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.PerWebRequest)
                {
                    container.Register(Component.For<IExceptionAdvice>().ImplementedBy(exceptionHandlerType).LifestylePerWebRequest().Named(exceptionHandlerType.FullName));
                }
            }

            var successHandlerTypes = AssemblyFinder.Impl.AssemblyFinder.Current.GetTypesOf<ISuccessAdvice>(assemblies);

            foreach (var successHandlerType in successHandlerTypes)
            {
                if (_lifestyleType == LifestyleType.Singleton)
                {
                    container.Register(Component.For<ISuccessAdvice>().ImplementedBy(successHandlerType).Named(successHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Scoped)
                {
                    container.Register(Component.For<ISuccessAdvice>().ImplementedBy(successHandlerType).LifestyleScoped().Named(successHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Transient)
                {
                    container.Register(Component.For<ISuccessAdvice>().ImplementedBy(successHandlerType).LifestyleTransient().Named(successHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.PerWebRequest)
                {
                    container.Register(Component.For<ISuccessAdvice>().ImplementedBy(successHandlerType).LifestylePerWebRequest().Named(successHandlerType.FullName));
                }
            }

            var entryHandlerTypes = AssemblyFinder.Impl.AssemblyFinder.Current.GetTypesOf<IEntryAdvice>(assemblies);

            foreach (var entryHandlerType in entryHandlerTypes)
            {
                if (_lifestyleType == LifestyleType.Singleton)
                {
                    container.Register(Component.For<IEntryAdvice>().ImplementedBy(entryHandlerType).Named(entryHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Scoped)
                {
                    container.Register(Component.For<IEntryAdvice>().ImplementedBy(entryHandlerType).LifestyleScoped().Named(entryHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.Transient)
                {
                    container.Register(Component.For<IEntryAdvice>().ImplementedBy(entryHandlerType).LifestyleTransient().Named(entryHandlerType.FullName));
                }
                if (_lifestyleType == LifestyleType.PerWebRequest)
                {
                    container.Register(Component.For<IEntryAdvice>().ImplementedBy(entryHandlerType).LifestylePerWebRequest().Named(entryHandlerType.FullName));
                }
            }

        }
    }
}