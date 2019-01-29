using System;
using System.Linq;
using System.Threading.Tasks;
using ABBMailing.Models;
using ABBMailing.Persistance;
using ABBMailing.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ABBMailing.Controllers
{
    [Route("api/[controller]")]
    public class AddressesController : Controller
    {
        private readonly MailingContext _context;

        public AddressesController(MailingContext context)
        {
            _context = context;
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
                await SaveAddress(newAddress);
                // send email
            }

            return Ok();
        }

        private async Task SaveAddress(AddressViewModel newAddress)
        {
            var topics = _context.Topics.Where(t => newAddress.Topics.Contains(t.Id));
            var address = new Address { Email = newAddress.Email.ToLower(), UnsubscribeToken = Guid.NewGuid().ToString() };
            var addressTopics = topics.Select(t => new AddressTopic {Topic = t, Address = address});
            
            _context.Addresses.Add(address);
            _context.AddressTopic.AddRange(addressTopics);

            await _context.SaveChangesAsync();
        }
    }
}