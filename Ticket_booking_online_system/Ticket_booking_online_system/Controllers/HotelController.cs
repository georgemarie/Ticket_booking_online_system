using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Ticket_booking_online_system.Controllers
{
    [Authorize(Roles = "Admin")] // Entire controller is Admin Only
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly ILocationRepository _locationRepo;

        public HotelController(IHotelRepository hotelRepo, IServiceRepository serviceRepo, ILocationRepository locationRepo)
        {
            _hotelRepo = hotelRepo;
            _serviceRepo = serviceRepo;
            _locationRepo = locationRepo;
        }

        public IActionResult Search()
        {
            var hotels = _hotelRepo.GetAllWithIncludes();
            return View(hotels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var services = _serviceRepo.GetAll().Distinct().ToList();
            ViewBag.ServiceId = new SelectList(services, "ServiceID", "ServiceType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HotelService model)
        {
            ModelState.Remove("Service.Location");

            if (!ModelState.IsValid)
            {
                ViewBag.ServiceId = new SelectList(_serviceRepo.GetAll().Distinct(), "Id", "ServiceType", model.ServiceId);
                ViewBag.LocationID = new SelectList(_locationRepo.GetAll(), "LocationID", "City");
                return View(model);
            }

            _hotelRepo.Add(model);
            _hotelRepo.Save();
            return RedirectToAction(nameof(Search));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();
            var hotel = _hotelRepo.GetById(id);
            if (hotel == null) return NotFound();
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HotelService hotel)
        {
            if (hotel == null) return BadRequest();
            if (ModelState.IsValid)
            {
                _hotelRepo.Update(hotel);
                _hotelRepo.Save();
                return RedirectToAction(nameof(Search));
            }
            return View(hotel);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var hotel = _hotelRepo.GetById(id);
            if (hotel == null) return NotFound();
            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hotel = _hotelRepo.GetById(id);
            if (hotel == null) return NotFound();

            _hotelRepo.Delete(hotel);
            _hotelRepo.Save();
            return RedirectToAction(nameof(Search));
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