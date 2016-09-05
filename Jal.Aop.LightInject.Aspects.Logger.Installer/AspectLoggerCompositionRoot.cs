using Jal.Aop.Aspects.Interface;
using Jal.Aop.Aspects.Logger;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Logger.Installer
{
    public class AspectLoggerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IAspectLogger, AspectLogger>(new PerContainerLifetime());
        }
    }
}
