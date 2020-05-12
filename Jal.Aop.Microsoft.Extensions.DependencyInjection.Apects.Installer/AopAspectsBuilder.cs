using Jal.Aop.Aspects;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
}
