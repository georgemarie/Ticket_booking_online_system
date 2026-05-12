using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightServiceRepository _flightRepo;
        private readonly IBookingService _bookingService;

        public FlightController(
            IFlightServiceRepository flightRepo,
            IBookingService bookingService)
        {
            _flightRepo = flightRepo;
            _bookingService = bookingService;
        }


        public IActionResult Search()
        {
            var vm = new FlightSearchVM
            {
                Flights = _flightRepo.GetAllWithIncludes().ToList()
            };

            return View(vm);
        }

        public IActionResult Results(string from, string to, DateTime date)
        {
            var vm = new FlightSearchVM
            {
                Flights = _flightRepo.Search(from, to, date).ToList()
            };

            return View("Search", vm);
        }
        public IActionResult Details(int serviceId)
        {
            var flight =
                _flightRepo.GetAllWithIncludes()
                .FirstOrDefault(f => f.ServiceId == serviceId);

            if (flight == null)
                return NotFound();

            return View(flight);
        }


    }
}
