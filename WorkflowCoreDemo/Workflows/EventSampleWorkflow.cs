using System;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace WorkflowCoreDemo.Workflows
{
    public class EventSampleWorkflow : IWorkflow<EventSampleWorkflowData>
    {
        public string Id => nameof(EventSampleWorkflow);
        public int Version => 1;

        public void Build(IWorkflowBuilder<EventSampleWorkflowData> builder)
        {
            // the workflow will wait for an event called "MyEvent" with a key of 0
            // Once an external source has fired this event, the workflow will wake up and continue processing,
            // passing the data generated by the event onto the next step.

            builder
            .StartWith(context => ExecutionResult.Next())
            .WaitFor("MyEvent", data => "0")
                .Output(data => data.OutputValue, step => step.EventData)
            .Then(context =>
            {
                Console.WriteLine("Done");
                return ExecutionResult.Next();
            });
        }

        //...
        // External events are published via the host
        // All workflows that have subscribed to MyEvent 0, will be passed "hello"
        // host.PublishEvent("MyEvent", "0", "hello");
        // or via api endpoint _workflowService.PublishEvent("MyEvent", "0", "hello");
    }

    public class EventSampleWorkflowData
    {
        public string? EventDataId { get; set; }
        public string? OutputValue { get; set; }
    }
}
