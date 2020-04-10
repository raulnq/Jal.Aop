using Jal.Aop.LightInject.Aspect.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jal.Aop.Aspects.Installer;
using LightInject;
using Jal.Aop.Aspects.Logger.Serilog;
using Jal.Aop.Aspects.Serializer.Json;
using Serilog;

namespace Jal.Aop.Tests.LightInject
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void Proxy_WithOneAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(new System.Type[] { typeof(Add) });

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithOneAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithMultipleAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(new System.Type[] { typeof(Add10), typeof(Multiple5), typeof(Subtract20), });

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithMultipleAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithAdviceAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(action:c=> { c.AddAdviceForAop<AddAdvice>(); });

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithAdviceAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithLogAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(action:c=> {
                c.AddLoggerForAop<SerilogLogger>();
                c.AddSerializerForAop<JsonSerializer>();
            });

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspect_ShoudBe(provider);
        }
    }
}
