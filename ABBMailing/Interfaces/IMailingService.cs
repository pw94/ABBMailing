using System.Collections.Generic;

namespace ABBMailing.Interfaces
{
    public interface IMailingService
    {
        void SendTopicsMail(string email, IEnumerable<string> topics, string token);
        void SendUnsubscribeConfirmation(string email);
    }
}