using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WorkflowCoreDemo.Services.Email
{
    public sealed record SendEmailRequest
    {
        public string DomainName { get; } = "WorkflowCoreDemo";

        public string Subject { get; init; } = null!;
        public string Body { get; init; } = null!;
        public List<EmailRecipient> Recipients { get; init; } = null!;
        public EmailAddress From { get; init; } = null!;
        public EmailAddress ReplyTo { get; init; } = null!;
    }

    public sealed record EmailRecipient : EmailAddress
    {
        public EmailRecipient(string email, string name) : base(email, name)
        {
            RecipientId = "RP" + Guid.NewGuid().ToString("N");
        }

        [JsonConstructor]
        public EmailRecipient(string recipientId, string email, string name) : base(email, name)
        {
            RecipientId = recipientId;
        }

        /// <summary>
        /// UniqueId. From Houses this would be the Member MDID
        /// </summary>
        public string RecipientId { get; }
    }

    public record EmailAddress
    {
        public EmailAddress(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; }
        public string Name { get; }
    }
}
