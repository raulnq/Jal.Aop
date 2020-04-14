# Jal.Aop [![NuGet](https://img.shields.io/nuget/v/Jal.Aop.svg)](https://www.nuget.org/packages/Jal.Aop) 
Just another library to do aspect oriented programming
## How to use?
Create your aspects
```csharp
public class AddAttribute : AbstractAspectAttribute
{
    public object[] Context { get; set; }
}

public class Add10Attribute : AbstractAspectAttribute
{

}

public class Multiple5Attribute : AbstractAspectAttribute
{

}

public class Subtract20Attribute : AbstractAspectAttribute
{

}

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
```
Use your aspects
```csharp
public interface INumberProvider
{
    int Get1(int seed);

    int Get2(int seed);

    int Get3(int seed);

    int Get4(int seed);
}

public class NumberProvider : INumberProvider
{
    [LoggerAspect(Type=typeof(SerilogLogger), LogArguments = new string[] { "seed" }, LogReturn =true, LogDuration =true, LogException =true)]
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
```
## Castle Windsor [![NuGet](https://img.shields.io/nuget/v/Jal.Aop.Aspects.Installer.svg)](https://www.nuget.org/packages/Jal.Aop.Aspects.Installer)

```csharp
 var container = new WindsorContainer();

 //should be declared before any registration in the container
container.AddAop(c =>
{
    c.AddAspect<Add10>();
    c.AddAspect<Multiple5>();
    c.AddAspect<Subtract20>();
});

container.Register(Component.For<INumberProvider>().ImplementedBy<NumberProvider>());

var provider = container.Resolve<INumberProvider>();

var seed = 5;

var value = provider.Get2(seed);
```

## LightInject [![NuGet](https://img.shields.io/nuget/v/Jal.Aop.LightInject.Aspect.Installer.svg)](https://www.nuget.org/packages/Jal.Aop.LightInject.Aspect.Installer)

```csharp
var container = new ServiceContainer();

container.Register<INumberProvider, NumberProvider>();

 //should be declared after all registrations in the container
container.AddAop(c =>
{
    c.AddAspect<Add10>();
    c.AddAspect<Multiple5>();
    c.AddAspect<Subtract20>();
});

var provider = container.GetInstance<INumberProvider>();

var seed = 5;

var value = provider.Get2(seed);
``` 

## Microsoft.Extensions.DependencyInjection [![NuGet](https://img.shields.io/nuget/v/Jal.Aop.Microsoft.Extensions.DependencyInjection.Apects.Installer.svg)](https://www.nuget.org/packages/Jal.Aop.Microsoft.Extensions.DependencyInjection.Apects.Installer)

```csharp
var container = new ServiceContainer();

container.AddSingleton<INumberProvider, NumberProvider>();

container.AddAop(c =>
{
    c.AddAspect<Add10>();
    c.AddAspect<Multiple5>();
    c.AddAspect<Subtract20>();
});

var p = container.BuildServiceProvider();

var provider = p.GetService<INumberProvider>();

var seed = 5;

var value = provider.Get2(seed);
``` 