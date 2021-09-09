using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using WorkflowCoreDemo.Workflows.HelloWorld;
using WorkflowCoreDemo.Workflows.HelloWorld.Data;

namespace WorkflowCoreDemo.Controllers
{
    [Route("[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowController _workflowService;
        private readonly IWorkflowRegistry _registry;
        private readonly IPersistenceProvider _workflowStore;

        public WorkflowsController(IWorkflowController workflowService, IWorkflowRegistry registry, IPersistenceProvider workflowStore)
        {
            _workflowService = workflowService;
            _registry = registry;
            _workflowStore = workflowStore;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _workflowStore.GetWorkflowInstance(id);
            return Ok(result);
        }

        [HttpGet("GetAllDefinitions")]
        public IActionResult GetAllDefinitions()
        {
            var definitions = _registry.GetAllDefinitions();
            return Ok(definitions);
        }

        [HttpPost("helloWorld")]
        public async Task<IActionResult> StartHelloWorldExampleWorkflow(HelloWorldDataInput input)
        {
            var result = await _workflowService.StartWorkflow(nameof(HelloWorldWorkflow), new HelloWorldData(input));
            return Ok(result);
        }

        [HttpPost("{id}")]
        [HttpPost("{id}/{version}")]
        public async Task<IActionResult> Post(string id, int? version, string reference, [FromBody] string data)
        {
            var def = _registry.GetDefinition(id, version);
            if (def == null)
            {
                return BadRequest($"Workflow defintion {id} for version {version} not found.");
            }

            string workflowId;
            if (!string.IsNullOrEmpty(data) && (def.DataType != null))
            {
                var dataObject = JsonSerializer.Deserialize(data, def.DataType);
                if (dataObject == null)
                {
                    return BadRequest($"Unable to Deserialize Workflow defintion {id} for version {version} DataType: {def.DataType} Data: {data}");
                }
                workflowId = await _workflowService.StartWorkflow(id, version, dataObject, reference);
            }
            else
            {
                workflowId = await _workflowService.StartWorkflow(id, version, null, reference);
            }

            return Ok(workflowId);
        }

        [HttpPut("{id}/suspend")]
        public async Task<bool> Suspend(string id) => await _workflowService.SuspendWorkflow(id);

        [HttpPut("{id}/resume")]
        public async Task<bool> Resume(string id) => await _workflowService.ResumeWorkflow(id);

        [HttpDelete("{id}")]
        public async Task<bool> Terminate(string id) => await _workflowService.TerminateWorkflow(id);
    }
}
