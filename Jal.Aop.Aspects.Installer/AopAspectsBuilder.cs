using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Jal.Aop.Aspects.Installer
{
    public class AopAspectsBuilder : IAopAspectsBuilder
    {
        private readonly IWindsorContainer _container;

        public List<Type> Types { get; }

        public AopAspectsBuilder(IWindsorContainer container, List<Type> types)
        {
            _container = container;

            Types = types;
        }

        public IAopAspectsBuilder AddAdvice<TImplementation>() where TImplementation : class, IAdvice
        {
            _container.Register(Component.For<IAdvice>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName));

            return this;
        }

        public IAopAspectsBuilder AddLogger<TImplementation>() where TImplementation : class, ILogger
        {
            _container.Register(Component.For<ILogger>().ImplementedBy<TImplementation>().Named(typeof(TImplementation).FullName));

            return this;
        }

        public IAopAspectsBuilder AddAspect<TImplementation>() where TImplementation : class, IAspect
        {
            var type = typeof(TImplementation);

            Types.Add(type);

            _container.Register(Component.For<IAspect>().ImplementedBy(type).Named(type.FullName).LifestyleTransient());

            return this;
        }
    }
}