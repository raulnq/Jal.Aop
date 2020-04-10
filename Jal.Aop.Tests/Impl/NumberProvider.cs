using Jal.Aop.Aspects;
using Jal.Aop.Aspects.Logger.Serilog;
using Jal.Aop.Aspects.Serializer.Json;

namespace Jal.Aop.Tests
{
    public class NumberProvider : INumberProvider
    {
        [LoggerAspect(LoggerType=typeof(SerilogLogger), SerializerType = typeof(JsonSerializer), LogArguments = true, LogReturn =true, LogDuration =true, LogException =true)]
        public int Get4(int seed)
        {
            return seed;
        }

        [AdviceAspect(Type = typeof(AddAdvice))]
        public int Get3(int seed)
        {
            return seed;
        }

        [Add(Context = new object[] { 10 })]
        public int Get1(int seed)
        {
            return seed;
        }

        [Add10(Order = 1)]
        [Multiple5(Order = 2)]
        [Subtract20(Order = 3)]
        public int Get2(int seed)
        {
            return seed;
        }
    }
}
