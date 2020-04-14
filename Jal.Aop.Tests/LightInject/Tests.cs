using Jal.Aop.LightInject.Aspect.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jal.Aop.Aspects.Installer;
using LightInject;
using Jal.Aop.Aspects.Logger.Serilog;
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

            container.AddAop(c =>
            {
                c.AddAspect<Add>();
            });

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithOneAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithMultipleAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(c=>
            {
                c.AddAspect<Add10>();
                c.AddAspect<Multiple5>();
                c.AddAspect<Subtract20>();
            });

            var provider = container.GetInstance<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithMultipleAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithAdviceAspect_ShoudBe()
        {
            var container = new ServiceContainer();

            container.Register<INumberProvider, NumberProvider>();

            container.AddAop(action:c=> { c.AddAdvice<AddAdvice>(); });

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
                c.AddLogger<SerilogLogger>();
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
