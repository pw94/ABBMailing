using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABBMailing.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ABBMailing.Services
{
    public class SendGridMailingService : IMailingService
    {
        private SendGridClient client;

        public SendGridMailingService()
        {
            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            this.client = new SendGridClient(apiKey);
        }
        public async Task SendTopicsMail(string email, IEnumerable<string> topics, string token)
        {
            var from = new EmailAddress("test@example.com", "Example User"); // take from settings
            var subject = "Thank you for subscirbing to our service!";
            var to = new EmailAddress(email);
            var plainTextContent = "and easy to do anywhere, even with C#"; // build the same way as html
            var htmlContent = this.BuildHtmlContent(topics);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await this.client.SendEmailAsync(msg);
        }

        private string BuildHtmlContent(IEnumerable<string> topics)
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
            // Add unsubscirbe link
            return builder.ToString();
        }

        public Task SendUnsubscribeConfirmation(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}