using WorkflowCore.Interface;
using WorkflowCoreDemo.Services.Email;
using WorkflowCoreDemo.Workflows.HelloWorld.Data;
using WorkflowCoreDemo.Workflows.HelloWorld.Steps;

namespace WorkflowCoreDemo.Workflows.HelloWorld
{
    // workflow definition with strongly typed internal data and mapped inputs & outputs
    public class HelloWorldWorkflow : IWorkflow<HelloWorldData>
    {
        public string Id => nameof(HelloWorldWorkflow); // These are used by the workflow host to identify a workflow definition
        public int Version => 1;

        public void Build(IWorkflowBuilder<HelloWorldData> builder)
        {
            // Each running workflow is persisted to the chosen persistence provider between each step,
            // where it can be picked up at a later point in time to continue execution.

            // The outcome result of your step can instruct the workflow host to defer further execution
            // of the workflow until a future point in time or in response to an external event.

            builder.StartWith<HelloWorldStep>()
                .Then<AddNumbers>()
                    .Input(step => step.Input1, data => data.Input.Value1)
                    .Input(step => step.Input2, data => data.Input.Value2)
                    .Output(data => data.Output.AddNumbersAnswer, step => step.Output)
                .Then<CustomMessage>()
                    .Input(step => step.Message, data => $"The answer is {data.Output.AddNumbersAnswer}")
                    .Output(data => data.Output.Message, step => step.Message)
                .Then<SendEmail>()
                    .Input(step => step.Body, data => data.Output.Message)
                    .Input(step => step.From, _ => new EmailAddress("test@example.com", "SentFromWorkFlow"))
                    .Input(step => step.To, data => new EmailRecipient(data.Input.Email ?? "", "Test Workflow"))
                .Then<GoodbyeWorld>();
        }
    }
}
