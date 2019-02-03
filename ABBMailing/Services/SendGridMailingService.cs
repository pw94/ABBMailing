using System;
using System.Collections.Generic;
using System.Text;
using ABBMailing.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ABBMailing.Services
{
    public class SendGridMailingService : IMailingService
    {
        private SendGridClient _client;
        private readonly EmailAddress _from;

        public SendGridMailingService(IConfiguration configuration)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            _client = new SendGridClient(apiKey);

            var fromName = configuration["SenderDetails:name"];
            var fromEmail = configuration["SenderDetails:email"];
            _from = new EmailAddress(fromEmail, fromName);
        }
        public void SendTopicsMail(string email, IEnumerable<string> topics, string unsubscribeLink)
        {
            var subject = "Thank you for subscribing to our service!";
            var to = new EmailAddress(email);
            var plainTextContent = BuildPlainTextContent(topics, unsubscribeLink);
            var htmlContent = BuildHtmlContent(topics, unsubscribeLink);
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
            _client.SendEmailAsync(msg).Wait();
        }

        private string BuildHtmlContent(IEnumerable<string> topics, string unsubscribeLink)
        {
            var builder = new StringBuilder();
            builder.Append("<strong>You subscribed to following topics:</strong>");
            builder.Append("<ul>");
            foreach (var topic in topics)
            {
                builder.Append("<li>");
                builder.Append(topic);
                builder.Append("</li>");
            }
            builder.Append("</ul>");
            builder.Append($"<a href=\"{unsubscribeLink}\">Unsubscribe</a>");
            return builder.ToString();
        }

        private string BuildPlainTextContent(IEnumerable<string> topics, string unsubscribeLink)
        {
            var builder = new StringBuilder();
            builder.Append("You subscribed to following topics:");
            builder.AppendLine();
            foreach (var topic in topics)
            {
                builder.Append(" - ");
                builder.Append(topic);
                builder.AppendLine();
            }
            builder.Append("You can unsubscribe using this link: ");
            builder.Append(unsubscribeLink);
            return builder.ToString();
        }

        public void SendUnsubscribeConfirmation(string email)
        {
            var subject = "You unsubscribed from our service";
            var to = new EmailAddress(email);
            var plainTextContent = "<p>You unsubscribed from our service.<p>";
            var htmlContent = "You unsubscribed from our service.";
            var msg = MailHelper.CreateSingleEmail(_from, to, subject, plainTextContent, htmlContent);
            _client.SendEmailAsync(msg).Wait();
        }
    }
}