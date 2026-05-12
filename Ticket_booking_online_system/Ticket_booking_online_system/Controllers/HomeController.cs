using BLL.Repository.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Ticket_booking_online_system.Models;

namespace Ticket_booking_online_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFlightServiceRepository
            _flightServiceRepo;

        private readonly IHotelRepository
            _hotelServiceRepo;

        public HomeController(
            IFlightServiceRepository flightServiceRepo,

            IHotelRepository hotelServiceRepo)
        {
            _flightServiceRepo = flightServiceRepo;

            _hotelServiceRepo = hotelServiceRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
