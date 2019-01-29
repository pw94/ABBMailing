using System.Linq;
using System.Threading.Tasks;
using ABBMailing.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace ABBMailing.Controllers
{
    [Route("api/[controller]")]
    public class UnsubscribeController: Controller
    {
        private readonly MailingContext _context;

        public UnsubscribeController(MailingContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{token}")]
        public async Task<IActionResult> Confirm(string token)
        {
            var address = _context.Addresses.SingleOrDefault(a => a.UnsubscribeToken == token);
            if (address?.Subscribed == true)
            {
                address.Subscribed = false;
                await _context.SaveChangesAsync();
                // send mail
            }
            return RedirectToPage("/Unsubscribed");
        }
    }
}