using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Aop.CastleWindsor;
using Jal.Locator.CastleWindsor;

namespace Jal.Aop.Aspects.Installer
{
    public class AspectInstaller : IWindsorInstaller
    {
        private readonly Type[] _type;

        private readonly bool _automaticInterception;

        private readonly Action<IWindsorContainer> _action;

        public AspectInstaller(Type[] type=null, Action<IWindsorContainer> action = null, bool automaticInterception=true)
        {
            _type = type;

            _automaticInterception = automaticInterception;

            _action = action;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddServiceLocator();

            if (_automaticInterception)
            {
                container.Kernel.ComponentModelBuilder.AddContributor(new AutomaticInterception());
            }

            var types = new List<Type>(_type ?? new Type[] { })
                              {
                                  typeof (LoggerAspect),
                                  typeof (AdviceAspect)
                              }.ToArray();

            container.Register(Component.For<IFactory<IAdvice>>().ImplementedBy<Factory<IAdvice>>());

            container.Register(Component.For<IFactory<ILogger>>().ImplementedBy<Factory<ILogger>>());

            container.Register(Component.For<IAspectExecutor>().ImplementedBy<AspectExecutor>().DependsOn(new { types = types }));

            container.Register(Component.For(typeof(AopProxy)).LifestyleTransient());

            container.Register(Component.For<IPointCut>().ImplementedBy<PointCut>());

            container.Register(Component.For<IFactory<ISerializer>>().ImplementedBy<Factory<ISerializer>>());

            container.Register(Component.For<ISerializer>().ImplementedBy<DataContractSerializer>().Named(typeof(DataContractSerializer).FullName));

            container.Register(Component.For<ISerializer>().ImplementedBy<XmlSerializer>().Named(typeof(XmlSerializer).FullName));

            foreach (var type in types)
            {
                if (!typeof(IAspect).IsAssignableFrom(type))
                {
                    throw new Exception($"The type {type.FullName} is a not valid IAspect implementation");
                }
                container.Register(Component.For<IAspect>().ImplementedBy(type).Named(type.FullName).LifestyleTransient());
            }

            container.Register(Component.For<IAdvice>().ImplementedBy<Advice>().Named(typeof(Advice).FullName));

            container.Register(Component.For<IExpressionEvaluator>().ImplementedBy<ExpressionEvaluator>());

            if(_action!=null)
            {
                _action(container);
            }
        }
    }
}