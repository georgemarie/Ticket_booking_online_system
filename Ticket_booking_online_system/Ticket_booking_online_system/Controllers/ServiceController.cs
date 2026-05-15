using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Service")]
    //[Authorize]
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

        // GET: Services
        //[AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index(string type = "Flight")
        {
            ViewBag.ActiveService = type;
            var services = _ServiceRepository.GetAll();

            var allCities = _LocationRepository.GetAll()
            .Where(loc => !string.IsNullOrEmpty(loc.City))
            .Select(loc => loc.City)
            .Distinct()
            .ToList();

            ViewBag.availableCities = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(allCities);

            return View(services);
        }

        [HttpPost("SearchFlights")] // Matches: /Services/SearchFlights
        public ActionResult SearchFlights(string from, string to, DateTime date)
        {
            var avaliableFlights = _FlightService.Search(from, to, date);
            if (!avaliableFlights.Any())
            {
                ViewBag.Message = "No Available No Flights found.";
                ViewBag.ActiveService = "Flight";
                return View("Index", _ServiceRepository.GetAll());
            }
            return View("SearchFlights", avaliableFlights);
        }
        [HttpPost("SearchHotels")] // Matches: /Services/SearchHotels
        public ActionResult SearchHotels(string city)
        {
            var avaliableHotels = _HotelRepository.Search(city); 
            if (!avaliableHotels.Any())
            {
                ViewBag.Message = "No Hotels found in that city.";
                ViewBag.ActiveService = "Hotel";
                return View("Index", _ServiceRepository.GetAll());
            }
            return View("SearchHotels", avaliableHotels);
        }

        // GET: Services/Details/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            var service = _ServiceRepository.GetById(id);
            return View(service);
        }

        #region Admin Controllers
        // GET: Services/Create
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public ActionResult Create()
        {
            var locations = _LocationRepository.GetAll();
            ViewBag.LocationID = new SelectList(locations, "LocationID", "City");
            return View();
        }

        // POST: Services/Create
        //[Authorize(Roles = "Admin")]
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

        // GET: Services/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id < 0) return BadRequest();
            var service = _ServiceRepository.GetById(id);
            if (service == null) return NotFound();
            return View(service);
        }

        // POST: Services/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            var editService = _ServiceRepository.GetById(service.ServiceID);
            if (editService == null) return NotFound();
            if (ModelState.IsValid)
            {
                _ServiceRepository.Update(editService);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(service);
            }
        }

        // GET: Services/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var service = _ServiceRepository.GetById(id);
            if (service == null) return NotFound();
            return View(service);
        }
        // POST: Services/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Service service)
        {
            var deletedService = _ServiceRepository.GetById(service.ServiceID);
            if (deletedService == null) return NotFound();
            if (ModelState.IsValid)
            {
                _ServiceRepository.Delete(deletedService);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(service);
            }
        } 
        #endregion
    }
}
