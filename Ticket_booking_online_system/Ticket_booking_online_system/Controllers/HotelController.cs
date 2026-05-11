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

        public IActionResult Index()
        {
            var hotels = _hotelRepo.GetAllHotels();
            return View(hotels);
        }

        public IActionResult Search(string city)
        {
            var hotels = _hotelRepo.SearchByCity(city);
            return View("Index", hotels);
        }
    }
}
