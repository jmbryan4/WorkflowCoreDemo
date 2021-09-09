using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreDemo.Workflows.HelloWorld.Steps
{
    // workflow step with inputs and outputs
    // Each step is intended to be a black-box, therefore they support inputs and outputs.
    // These inputs and outputs can be mapped to a data class that defines the custom data relevant to each workflow instance
    public class AddNumbers : StepBody
    {
        public int Input1 { get; init; }

        public int Input2 { get; init; }

        public int Output { get; set; }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Output = (Input1 + Input2);
            return ExecutionResult.Next();
        }
    }

    public class CustomMessage : StepBody
    {
        public string? Message { get; init; }

        public override ExecutionResult Run(IStepExecutionContext context) => ExecutionResult.Next();
    }
}
