using BLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightRepository _flightRepo;

        public FlightController(IFlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
        }

        // صفحة البحث
        public IActionResult SearchPage()
        {
            return View();
        }

        // نتيجة البحث
        public IActionResult Search(string from, string to, DateTime date)
        {
           

            var flights = _flightRepo.Search(from, to, date);
           

            return View(flights);
        }
    }
}
