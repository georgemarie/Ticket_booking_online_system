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

        public IActionResult Index(int userId=2)
        {
            var bookings = _bookingService.GetUserBookings(userId);
            return View(bookings);
        }


        

        [HttpPost]
        public IActionResult CreateBooking(int serviceId)
        {
            bool result =
                _bookingService
                    .CreateBooking(serviceId);

            if (!result)
                return Content("Booking Failed");

            return RedirectToAction(
                "Index",
                "Booking");
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