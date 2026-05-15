using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Service")]
    public class ServiceController : Controller
    {
        private IServiceRepository _ServiceRepository { get; }
        private IFlightServiceRepository _FlightService { get; }
        private IHotelRepository _HotelRepository { get; }
        private ILocationRepository _LocationRepository { get; }

        public ServiceController(IServiceRepository serviceRepository, IFlightServiceRepository flightService, IHotelRepository hotelRepository, ILocationRepository locationRepository)
        {
            _ServiceRepository = serviceRepository;
            _FlightService = flightService;
            _HotelRepository = hotelRepository;
            _LocationRepository = locationRepository;
        }

        // --- PUBLIC SEARCH FLOW ---
        [AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index(string type = "Portal")
        {
            ViewBag.ActiveService = type;
            var services = _ServiceRepository.GetAll();

            var allCities = _LocationRepository.GetAll()
                .Where(loc => !string.IsNullOrEmpty(loc.City))
                .Select(loc => loc.City).Distinct().ToList();

            ViewBag.availableCities = new SelectList(allCities);
            return View(services);
        }

        [AllowAnonymous]
        [HttpPost("SearchFlights")]
        public ActionResult SearchFlights(string from, string to, DateTime date)
        {
            var avaliableFlights = _FlightService.Search(from, to, date);
            if (!avaliableFlights.Any())
            {
                ViewBag.Message = "No Available Flights found.";
                ViewBag.ActiveService = "Flight";
                ViewBag.availableCities = new SelectList(_LocationRepository.GetAll().Select(l => l.City).Distinct().ToList());
                return View("Index", _ServiceRepository.GetAll());
            }
            return View("SearchFlights", avaliableFlights);
        }

        [AllowAnonymous]
        [HttpPost("SearchHotels")]
        public ActionResult SearchHotels(string city)
        {
            var avaliableHotels = _HotelRepository.Search(city);
            if (!avaliableHotels.Any())
            {
                ViewBag.Message = "No Hotels found in that city.";
                ViewBag.ActiveService = "Hotel";
                ViewBag.availableCities = new SelectList(_LocationRepository.GetAll().Select(l => l.City).Distinct().ToList());
                return View("Index", _ServiceRepository.GetAll());
            }
            return View("SearchHotels", avaliableHotels);
        }

        [AllowAnonymous]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            var service = _ServiceRepository.GetById(id);
            return View(service);
        }

        // --- ADMIN CRUD FLOW ---
        [Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public ActionResult Create()
        {
            var locations = _LocationRepository.GetAll();
            ViewBag.LocationID = new SelectList(locations, "LocationID", "City");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _ServiceRepository.Add(service);
                return RedirectToAction(nameof(Index));
            }
            var locations = _LocationRepository.GetAll();
            ViewBag.LocationID = new SelectList(locations, "LocationID", "City", service.LocationID);
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id < 0) return BadRequest();
            var service = _ServiceRepository.GetById(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                _ServiceRepository.Update(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var service = _ServiceRepository.GetById(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Service service)
        {
            var deletedService = _ServiceRepository.GetById(service.ServiceID);
            if (deletedService == null) return NotFound();

            _ServiceRepository.Delete(deletedService);
            return RedirectToAction(nameof(Index));
        }
    }
}