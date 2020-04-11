using Jal.Aop.Aspects;
using Jal.Aop.Aspects.Logger.Serilog;

namespace Jal.Aop.Tests
{
    public class NumberProvider : INumberProvider
    {
        [LoggerAspect(Type=typeof(SerilogLogger), LogArguments = new string[] { "seed" }, LogReturn =true, LogDuration =true, LogException =true)]
        public int Get4(int seed)
        {
            return seed;
        }

        [LoggerAspect(Expression= "id", Type = typeof(SerilogLogger), LogArguments = new string[] { "seed","id" }, LogReturn = true, LogDuration = true, LogException = true)]
        public int Get5(int seed, string id)
        {
            return seed;
        }

        [LoggerAspect(Expression = "parameter.Id", Type = typeof(SerilogLogger), LogArguments = new string[] { "seed", "parameter" }, LogReturn = true, LogDuration = true, LogException = true)]
        public int Get6(int seed, Parameter parameter)
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
