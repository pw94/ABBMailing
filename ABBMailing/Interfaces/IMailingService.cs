using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABBMailing.Interfaces
{
    public interface IMailingService
    {
        Task SendTopicsMail(string email, IEnumerable<string> topics, string token);
        Task SendUnsubscribeConfirmation(string email);
    }
}