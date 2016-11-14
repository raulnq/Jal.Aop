using System.Reflection;
using Jal.Aop.Aspects.Correlation;
using Jal.Aop.Aspects.Interface;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Correlation.Installer
{
    public static class ServiceContainerExtension
    {
        public static void RegisterCorrelation(this IServiceContainer container, Assembly[] assemblies=null)
        {
            if (assemblies != null)
            {
                foreach (var assembly in assemblies)
                {
                    foreach (var exportedType in assembly.ExportedTypes)
                    {
                        if (typeof(ICorrelationIdProvider).IsAssignableFrom(exportedType))
                        {
                            container.Register(typeof(ICorrelationIdProvider), exportedType, exportedType.FullName, new PerContainerLifetime());
                        }
                    }
                }
            }

            container.Register<ICorrelationIdProviderFactory, CorrelationIdProviderFactory>(new PerContainerLifetime());
        }
    }
}