using System.Linq;
using LightInject;
using LightInject.Interception;

namespace Jal.Aop.LightInject
{
    public class AutomaticInterceptionCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Intercept(Match, Apply);
        }

        private bool Match(ServiceRegistration sr)
        {
            if (sr.ImplementingType != null)
            {
                var methods = sr.ImplementingType.GetMethods();

                if (methods.Select(methodInfo => methodInfo.GetCustomAttributes(typeof(AbstractAspectAttribute), true)).Any(attributes => attributes.Length > 0))
                {
                    return true;
                }
            }

            return false;
        }

        private void Apply(IServiceFactory factory, ProxyDefinition proxyDefinition)
        {
            proxyDefinition.Implement(factory.GetInstance<AopProxy>);
        }
    }
}
