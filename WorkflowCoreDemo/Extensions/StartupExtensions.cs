using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkflowCore.Interface;
using WorkflowCoreDemo.Services.Email;
using WorkflowCoreDemo.Workflows.HelloWorld;
using WorkflowCoreDemo.Workflows.HelloWorld.Data;
using WorkflowCoreDemo.Workflows.HelloWorld.Steps;

namespace WorkflowCoreDemo.Extensions
{
    public static class StartupExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IEmailClient, EmailClient>(c => c.BaseAddress = new Uri(configuration["EmailApiBaseUrl_Dev"]))
                    .SetHandlerLifetime(TimeSpan.FromMinutes(10));

            // add workflow steps to container
            services.AddTransient<SendEmail>();
        }

        public static void RegisterWorkflows(this IApplicationBuilder app, ILogger<Startup> logger)
        {
            // The workflow host is the service responsible for executing workflows.
            // It does this by polling the persistence provider for workflow instances that are ready to run,
            // executes them and then passes them back to the persistence provider to by stored for the next time they are run.
            // It is also responsible for publishing events to any workflows that may be waiting on one.
            var host = app.ApplicationServices.GetService<IWorkflowHost>();
            if (host == null)
            {
                logger.LogError("WorkflowHost not found.");
                return;
            }
            host.RegisterWorkflow<HelloWorldWorkflow, HelloWorldData>();
            host.Start(); // fire up the thread pool that executes workflows
            // optionally start any workflows that should run on startup
            //var input = new HelloWorldDataInput { Value1 = 21, Value2 = 2, Email = "test@example.com" };
            //host.StartWorkflow(nameof(HelloWorldWorkflow), new HelloWorldData(input));
        }
    }
}
