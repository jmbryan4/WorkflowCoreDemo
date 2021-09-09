namespace WorkflowCoreDemo.Workflows.HelloWorld.Data
{
    // class to define the internal data of our HelloWorld workflow
    public class HelloWorldData
    {
        public HelloWorldData()
        {
        }
        public HelloWorldData(HelloWorldDataInput input)
        {
            Input = input;
        }
        public HelloWorldDataInput Input { get; init; } = new();
        public HelloWorldDataOutput Output { get; init; } = new();
    }

    public class HelloWorldDataInput
    {
        public int Value1 { get; init; }
        public int Value2 { get; init; }
        public string? Email { get; init; }
    }

    public class HelloWorldDataOutput
    {
        public int AddNumbersAnswer { get; init; }
        public string? Message { get; init; }
    }
}
