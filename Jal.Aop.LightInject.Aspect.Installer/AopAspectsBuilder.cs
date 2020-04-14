using System;
using System.Collections.Generic;
using Jal.Aop.Aspects;
using LightInject;

namespace Jal.Aop.LightInject.Aspect.Installer
{
    public class AopAspectsBuilder : IAopAspectsBuilder
    {
        private readonly IServiceContainer _container;

        public List<Type> Types { get; }

        public AopAspectsBuilder(IServiceContainer container, List<Type> types)
        {
            _container = container;

            Types = types;
        }

        public IAopAspectsBuilder AddAdvice<TImplementation>() where TImplementation : class, IAdvice
        {
            _container.Register<IAdvice, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IAopAspectsBuilder AddLogger<TImplementation>() where TImplementation : class, ILogger
        {
            _container.Register<ILogger, TImplementation>(typeof(TImplementation).FullName, new PerContainerLifetime());

            return this;
        }

        public IAopAspectsBuilder AddAspect<TImplementation>() where TImplementation : class, IAspect
        {
            var type = typeof(TImplementation);

            Types.Add(type);

            _container.Register(typeof(IAspect), type, type.FullName);

            return this;
        }
    }
}