using Shouldly;

namespace Jal.Aop.Tests
{
    public class TestCases
    {
        public void Proxy_WithLogAspect_ShoudBe(INumberProvider provider)
        {
            var seed = 0;

            var value = provider.Get4(seed);

            value.ShouldBe(0);
        }

        public void Proxy_WithAdviceAspect_ShoudBe(INumberProvider provider)
        {
            var seed = 0;

            var value = provider.Get3(seed);

            value.ShouldBe(15);
        }

        public void Proxy_WithOneAspect_ShoudBe(INumberProvider provider)
        {
            var seed = 5;

            var value = provider.Get1(seed);

            value.ShouldBe(15);
        }

        public void Proxy_WithMultipleAspect_ShoudBe(INumberProvider provider)
        {
            var seed = 5;

            var value = provider.Get2(seed);

            value.ShouldBe(-65);
        }
    }
}
