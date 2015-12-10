# Jal.Aop
Just another library to do aspect oriented programming (based on Castle Windsor)
## How to use?
Note: The AssemblyFinder library is needed.
Setup the AssemblyFinder library.

    var directory = AppDomain.CurrentDomain.BaseDirectory;
    AssemblyFinder.Impl.AssemblyFinder.Current = new AssemblyFinder.Impl.AssemblyFinder(directory);
    
Setup the Castle Windsor container.

    var container = new WindsorContainer();
    container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

Add the contributor for Castle Windsor and the installer.

    container.Kernel.ComponentModelBuilder.AddContributor(new AutomaticInterception());
    container.Install(new AspectInstaller());
    
Create your aspect.

    public class MethodBoundaryAspectAttribute : AbstractAspectAttribute
    {
        
    }
    
    public class MethodBoundaryAspect : OnMethodBoundaryAspect<MethodBoundaryAspectAttribute>
    {
        protected override void OnEntry(IJoinPoint invocation)
        {
            Console.WriteLine("OnEntry " + invocation.MethodInfo);
        }

        protected override void OnExit(IJoinPoint invocation)
        {
            Console.WriteLine("OnExit "+ invocation.MethodInfo);
        }
    }
    
Mark your target class.

    public class DumbClass : IDumbClass
    {
        [MethodBoundaryAspect()]
        public void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }

Register your class

    container.Register(Component.For<IDumbClass>().ImplementedBy<DumbClass>());
  
Resolve and use instance of the interface IDumbClass.

    var dumbClass = container.Resolve<IDumbClass>();
    dumbClass.PrintMessage("Hi Aop!!");
