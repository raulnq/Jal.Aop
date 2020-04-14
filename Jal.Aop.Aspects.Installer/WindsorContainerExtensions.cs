using System;
using System.Collections.Generic;
using Castle.Windsor;
using Jal.Locator.CastleWindsor;

namespace Jal.Aop.Aspects.Installer
{


    public static class WindsorContainerExtensions
    {
        public static void AddAop(this IWindsorContainer container, Action<IAopAspectsBuilder> action = null, bool automaticInterception = true)
        {
            container.AddServiceLocator();

            var types = new List<Type>();

            var builder = new AopAspectsBuilder(container, types);

            builder.AddAspect<LoggerAspect>();

            builder.AddAspect<AdviceAspect>();

            if (action != null)
            {
                action(builder);
            }

            container.Install(new AspectInstaller(builder.Types.ToArray(), automaticInterception));


        }
    }
}