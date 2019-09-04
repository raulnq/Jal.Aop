using Jal.Aop.Aspects.Interface;
using Jal.Aop.Aspects.Logger.Serilog;
using LightInject;

namespace Jal.Aop.LightInject.Aspects.Logger.Serilog.Installer
{
    public class SerilogAspectLoggerCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IAspectLogger, SerilogAspectLogger>(new PerContainerLifetime());
        }
    }
}
