using System.Collections.Generic;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Models;
using WorkflowCoreDemo.Services.Email;

namespace WorkflowCoreDemo.Workflows.HelloWorld.Steps
{
    public class SendEmail : StepBodyAsync
    {
        private readonly IEmailClient _emailApiClient;

        public SendEmail(IEmailClient emailApiClient)
        {
            _emailApiClient = emailApiClient;
        }

        public string Subject { get; } = "Sent from a WorkFlow Step";
        public string Body { get; init; } = null!;
        public EmailRecipient To { get; init; } = null!;
        public EmailAddress From { get; init; } = null!;

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            var sendEmailRequest = new SendEmailRequest()
            {
                Subject = Subject,
                Body = Body,
                Recipients = new List<EmailRecipient>(1) { To },
                From = From,
                ReplyTo = From
            };
            await _emailApiClient.SendEmailAsync(sendEmailRequest).ConfigureAwait(false);

            return ExecutionResult.Next();
        }
    }
}
