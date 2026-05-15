using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket_booking_online_system.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly ILocationRepository _locationRepo;


        public HotelController(IHotelRepository hotelRepo, IServiceRepository serviceRepo, ILocationRepository location)
        {
            _hotelRepo = hotelRepo;
            _serviceRepo = serviceRepo;
        }


        #region  Admin Responsibilities
        #region  EditHotel 
        // GET: Edit
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var hotel = _hotelRepo.GetById(id);

            if (hotel == null)
                return NotFound();

            return View(hotel);
        }

        // POST: Edit
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HotelService hotel)
        {
            if (hotel == null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                _hotelRepo.Update(hotel);
                _hotelRepo.Save();

                return RedirectToAction(nameof(Search));
            }

            return View(hotel);
        }

        #endregion

        #region  AddingHotel
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var services = _serviceRepo.GetAll().ToList();

            ViewBag.ServiceId = new SelectList(services, "ServiceID", "ServiceType");

            return View();


        }
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HotelService model)
        {
            ModelState.Remove("Service.Location");

            if (!ModelState.IsValid)
            {
                ViewBag.ServiceId = new SelectList(_serviceRepo.GetAll(), "Id", "ServiceType", model.ServiceId);
                ViewBag.LocationID = new SelectList(_locationRepo.GetAll(), "LocationID", "City");
                return View(model);
            }

            _hotelRepo.Add(model);
            _hotelRepo.Save();
            return RedirectToAction(nameof(Search));
        }

        #endregion


        #region Delete Hotel
        // GET: Delete
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var hotel = _hotelRepo.GetById(id);

            if (hotel == null)
                return NotFound();

            return View(hotel);
        }

        // POST: Delete
        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hotel = _hotelRepo.GetById(id);

            if (hotel == null)
                return NotFound();

            _hotelRepo.Delete(hotel);
            _hotelRepo.Save();

            return RedirectToAction(nameof(Search));
        }



        #endregion


        #endregion


        public IActionResult Search()
        {
            var hotels = _hotelRepo.GetAllWithIncludes();

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