using BLL.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepo;

        public HotelController(IHotelRepository hotelRepo)
        {
            _hotelRepo = hotelRepo;
        }

        public IActionResult Search()
        {
            var hotels = _hotelRepo.GetAllWithIncludes().ToList();

            return View(hotels);
        }

        public IActionResult Results(string city)
        {
            var hotels = _hotelRepo.Search(city);

            return View("Search", hotels);
        }

        public IActionResult Details(int serviceId)
        {
            var hotel =
                _hotelRepo.GetAllWithIncludes()
                .FirstOrDefault(h => h.ServiceId == serviceId);

            if (hotel == null)
                return NotFound();

            return View(hotel);
        }
    }
}
