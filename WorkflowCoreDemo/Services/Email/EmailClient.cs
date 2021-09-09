using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WorkflowCoreDemo.Services.Email
{
    public class EmailClient : IEmailClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public EmailClient(HttpClient client, ILogger<EmailClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        private enum Endpoint
        {
            Status,
            SendEmail,
            MassEmail
        }

        public Task<string> PingAsync() => _client.GetStringAsync(nameof(Endpoint.Status));

        public async Task<bool> SendEmailAsync(SendEmailRequest emailRequest)
        {
            _logger.LogInformation("Sending email to {@ToEmails} {Subject}", emailRequest.Recipients, emailRequest.Subject);
            var endpoint = emailRequest.Recipients.Count > 1
                ? nameof(Endpoint.MassEmail)
                : nameof(Endpoint.SendEmail);
            var response = await _client.PostAsJsonAsync(endpoint, emailRequest).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogDebug("SendEmailAsync response: {ResponseJson}", responseJson);
            return response.IsSuccessStatusCode;
        }
    }

    public interface IEmailClient
    {
        Task<string> PingAsync();
        Task<bool> SendEmailAsync(SendEmailRequest emailRequest);
    }
}
