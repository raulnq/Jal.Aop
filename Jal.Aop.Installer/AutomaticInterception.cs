using System.Linq;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder;
using Jal.Aop.Impl;

namespace Jal.Aop.Installer
{
    public class AutomaticInterception : IContributeComponentModelConstruction
    {
        public void ProcessModel(IKernel kernel, ComponentModel model)
        {
            var methods = model.Implementation.GetMethods();

            if (methods.Select(methodInfo => methodInfo.GetCustomAttributes(typeof(AbstractAspectAttribute), true)).Any(attributes => attributes.Length > 0))
            {
                model.Interceptors.Add(new InterceptorReference(typeof(AopProxy)));
            }
        }
    }
}
