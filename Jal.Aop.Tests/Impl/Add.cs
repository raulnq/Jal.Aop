namespace Jal.Aop.Tests
{

    public class Add : OnMethodBoundaryAspect<AddAttribute>
    {
        protected override void OnExit(IJoinPoint joinPoint)
        {
            var context = CurrentAttribute.Context;

            var add = (int)context[0];

            var value = (int)joinPoint.Return + add;

            joinPoint.Return = value;
        }
    }

    public class Add10 : OnMethodBoundaryAspect<Add10Attribute>
    {
        protected override void OnExit(IJoinPoint joinPoint)
        {
            var value = (int)joinPoint.Return + 10;

            joinPoint.Return = value;
        }
    }

    public class Multiple5 : OnMethodBoundaryAspect<Multiple5Attribute>
    {
        protected override void OnExit(IJoinPoint joinPoint)
        {
            var value = (int)joinPoint.Return * 5;

            joinPoint.Return = value;
        }
    }

    public class Subtract20 : OnMethodBoundaryAspect<Subtract20Attribute>
    {
        protected override void OnExit(IJoinPoint joinPoint)
        {
            var value = (int)joinPoint.Return - 20;

            joinPoint.Return = value;
        }
    }
}
