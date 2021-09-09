using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreDemo.Workflows.HelloWorld.Steps
{
    // Steps are defined by creating a class that inherits from the StepBody or
    // StepBodyAsync abstract classes and implementing the Run/RunAsync method.
    // They can also be created inline while defining the workflow structure.

    // The StepBody and StepBodyAsync class implementations are constructed by the workflow host
    // which first tries to use IServiceProvider for dependency injection,
    // if it can't construct it with this method, it will search for a parameterless constructor.
    public class HelloWorldStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Hello world");
            return ExecutionResult.Next();
        }
    }

    public class GoodbyeWorld : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Goodbye world");
            return ExecutionResult.Next();
        }
    }
}
