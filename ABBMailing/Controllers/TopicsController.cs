using ABBMailing.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace ABBMailing.Controllers
{
    [Route("api/[controller]")]
    public class TopicsController : Controller
    {
        private readonly MailingContext _context;

        public TopicsController(MailingContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public IActionResult List()
        {
            return Json(_context.Topics);
        }
    }
}
