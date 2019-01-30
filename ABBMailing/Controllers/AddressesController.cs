using System;
using System.Linq;
using System.Threading.Tasks;
using ABBMailing.Interfaces;
using ABBMailing.Models;
using ABBMailing.Persistance;
using ABBMailing.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ABBMailing.Controllers
{
    [Route("api/[controller]")]
    public class AddressesController : Controller
    {
        private readonly MailingContext _context;
        private readonly IMailingService _mailingService;

        public AddressesController(MailingContext context, IMailingService mailingService)
        {
            _context = context;
            _mailingService = mailingService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddressViewModel newAddress)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            if (!_context.Addresses.Any(a => a.Email == newAddress.Email))
            {
                var topics = _context.Topics.Where(t => newAddress.Topics.Contains(t.Id));
                var newToken = Guid.NewGuid().ToString();

                await SaveAddress(newAddress, topics, newToken);
                await SendMail(newAddress.Email, topics.Select(t => t.Name), newToken);
            }

            return Ok();
        }

        private async Task SaveAddress(AddressViewModel newAddress, IQueryable<Topic> topics, string token)
        {
            var address = new Address { Email = newAddress.Email.ToLower(), UnsubscribeToken = token };
            var addressTopics = topics.Select(t => new AddressTopic {Topic = t, Address = address});
            
            _context.Addresses.Add(address);
            _context.AddressTopic.AddRange(addressTopics);

            await _context.SaveChangesAsync();
        }

        private async Task SendMail(string email, IQueryable<string> topicNames, string token)
        {
            var unsubscribeAddress = Url.Action("Confirm", "Unsubscribe", new { token = token }, Request.Scheme);
            await _mailingService.SendTopicsMail(email, topicNames, unsubscribeAddress);
        }
    }
}