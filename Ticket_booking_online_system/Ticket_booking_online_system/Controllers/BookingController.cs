using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using BLL.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Ticket_booking_online_system.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IFlightRepository _flightRepo;

        public BookingController(
            IBookingService bookingService,
            IFlightRepository flightRepo)
        {
            _bookingService = bookingService;
            _flightRepo = flightRepo;
        }

        public IActionResult Index(int userId)
        {
            var bookings = _bookingService.GetUserBookings(userId);
            return View(bookings);
        }


        public IActionResult Create(string flightNumber)
        {
            var flight = _flightRepo
        .GetAllWithIncludes()
        .FirstOrDefault(f => f.Flight_Number == flightNumber);

            if (flight == null)
                return NotFound();

            return View(flight);

           
        }


        [HttpPost]
        public IActionResult CreateBooking(string flightNumber)
        {
            bool result = _bookingService.CreateBooking(flightNumber);

            if (!result)
                return Content("Booking Failed ❌");

            return RedirectToAction("Index", new { userId = 2 });
        }


        public IActionResult Cancel(int id, int userId)
        {
            bool result = _bookingService.CancelBooking(id);

            if (!result)
                return Content("Cancel Failed ❌");

            return RedirectToAction("Index", new { userId = userId });
        }
    }
}