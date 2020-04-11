using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jal.Aop.Aspects.Installer;
using Serilog;
using Jal.Aop.Aspects.Logger.Serilog;

namespace Jal.Aop.Tests.CastleWindsor
{
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public void Proxy_WithOneAspect_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(new System.Type[] { typeof(Add) });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            var provider = container.Resolve<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithOneAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithMultipleAspect_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(new System.Type[] { typeof(Add10), typeof(Multiple5), typeof(Subtract20) });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            var provider = container.Resolve<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithMultipleAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithAdviceAspect_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(action: c => { c.AddAdviceForAop<AddAdvice>(); });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            var provider = container.Resolve<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithAdviceAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithLogAspect_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(action: c => {
                c.AddLoggerForAop<SerilogLogger>();
            });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var provider = container.Resolve<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspect_ShoudBe(provider);
        }


        [TestMethod]
        public void Proxy_WithLogAspectAndExpression_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(action: c => {
                c.AddLoggerForAop<SerilogLogger>();
            });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var provider = container.Resolve<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspectAndExpression_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithLogAspectAndComplexExpression_ShoudBe()
        {
            var container = new WindsorContainer();

            container.AddAop(action: c => {
                c.AddLoggerForAop<SerilogLogger>();
            });

            container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var provider = container.Resolve<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspectAndComplexExpression_ShoudBe(provider);
        }
    }
}
