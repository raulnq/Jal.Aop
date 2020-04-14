using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.CastleWindsor;

namespace Jal.Aop.Aspects.Installer
{
    public class AspectInstaller : IWindsorInstaller
    {
        private readonly Type[] _type;

        private readonly bool _automaticInterception;

        public AspectInstaller(Type[] type=null,  bool automaticInterception=true)
        {
            _type = type;

            _automaticInterception = automaticInterception;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (_automaticInterception)
            {
                container.Kernel.ComponentModelBuilder.AddContributor(new AutomaticInterception());
            }

            container.Register(Component.For<IFactory<IAdvice>>().ImplementedBy<Factory<IAdvice>>());

            container.Register(Component.For<IFactory<ILogger>>().ImplementedBy<Factory<ILogger>>());

            container.Register(Component.For<IAspectExecutor>().ImplementedBy<AspectExecutor>().DependsOn(new { types = _type }));

            container.Register(Component.For(typeof(AopProxy)).LifestyleTransient());

            container.Register(Component.For<IPointCut>().ImplementedBy<PointCut>());

            container.Register(Component.For<IAdvice>().ImplementedBy<Advice>().Named(typeof(Advice).FullName));

            container.Register(Component.For<IEvaluator>().ImplementedBy<Evaluator>());
        }
    }
}