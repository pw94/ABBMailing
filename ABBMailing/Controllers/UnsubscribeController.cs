using System.Linq;
using System.Threading.Tasks;
using ABBMailing.Interfaces;
using ABBMailing.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace ABBMailing.Controllers
{
    [Route("api/[controller]")]
    public class UnsubscribeController: Controller
    {
        private readonly MailingContext _context;
        private readonly IMailingService _mailingService;

        public UnsubscribeController(MailingContext context, IMailingService mailingService)
        {
            _context = context;
            _mailingService = mailingService;
        }

        [HttpGet("[action]/{token}")]
        public async Task<IActionResult> Confirm(string token)
        {
            var address = _context.Addresses.SingleOrDefault(a => a.UnsubscribeToken == token);
            if (address?.Subscribed == true)
            {
                address.Subscribed = false;
                await _context.SaveChangesAsync();
                await _mailingService.SendUnsubscribeConfirmation(address.Email);
            }
            return RedirectToPage("/Unsubscribed");
        }
    }
}