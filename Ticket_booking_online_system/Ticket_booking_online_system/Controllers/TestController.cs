using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
