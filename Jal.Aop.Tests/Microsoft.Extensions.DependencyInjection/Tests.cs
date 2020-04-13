using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jal.Aop.Aspects.Installer;
using Serilog;
using Jal.Aop.Apects.Microsoft.Extensions.DependencyInjection.Installer;
using Microsoft.Extensions.DependencyInjection;
using Jal.Aop.Aspects.Logger.Serilog;

namespace Jal.Aop.Tests.Microsoft.Extensions.DependencyInjection
{
    [TestClass]
    public class Tests
    {

        [TestMethod]
        public void Proxy_WithOneAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddAop(new System.Type[] { typeof(Add) }, c=>
            {
                c.AddSingleton<INumberProvider, NumberProvider>();
            });

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithOneAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithMultipleAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddAop(new System.Type[] { typeof(Add10), typeof(Multiple5), typeof(Subtract20) }, c =>
            {
                c.AddSingleton<INumberProvider, NumberProvider>();
            });

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var test = new TestCases();

            test.Proxy_WithMultipleAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithAdviceAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddAop(action: c => { 
                c.AddAdviceForAop<AddAdvice>();
                c.AddSingleton<INumberProvider, NumberProvider>();
            });

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithAdviceAspect_ShoudBe(provider);
        }

        [TestMethod]
        public void Proxy_WithLogAspect_ShoudBe()
        {
            var container = new ServiceCollection();

            container.AddAop(action: c =>
            {
                c.AddLoggerForAop<SerilogLogger>();
                c.AddSingleton<INumberProvider, NumberProvider>();
            });

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}{Properties}").MinimumLevel.Verbose()
            .CreateLogger();

            var p = container.BuildServiceProvider();

            var provider = p.GetService<INumberProvider>();

            var tests = new TestCases();

            tests.Proxy_WithLogAspect_ShoudBe(provider);
        }
    }
}
