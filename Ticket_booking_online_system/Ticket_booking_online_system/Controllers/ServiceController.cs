using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Services")]
    //[Authorize]
    public class ServiceController : Controller
    {
        public IServiceRepository _ServiceRepository { get; }
        public IFlightServiceRepository _FlightService { get; }
        public IHotelRepository _HotelRepository { get; }

        public ServiceController(IServiceRepository serviceRepository, IFlightServiceRepository flightService, IHotelRepository hotelRepository)
        {
            _ServiceRepository = serviceRepository;
            _FlightService = flightService;
            _HotelRepository = hotelRepository;
        }
        //[AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("SearchFlights")] // Matches: /Services/SearchFlights
        public ActionResult SearchFlights(string from, string to, DateTime date)
        {
            var avaliableFlights = _FlightService.Search(from, to, date);
            if (!avaliableFlights.Any())
            {
                ViewBag.Message = "No Available No Flights found.";
                return View("Index");
            }
            return View("SearchFlights", avaliableFlights);
        }
        [HttpPost("SearchHotels")] // Matches: /Services/SearchHotels
        public ActionResult SearchHotels(string city)
        {
            var avaliableHotels = _HotelRepository.Search(city); 
            if (!avaliableHotels.Any())
            {
                ViewBag.Message = "No Available No Hotels found.";
                return View("Index");
            }
            return View("SearchHotels", avaliableHotels);
        }

        // GET: ServiceController/Details/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            var service = _ServiceRepository.GetById(id);
            return View(service);
        }

        // GET: ServiceController/Create
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiceController/Create
        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            if(ModelState.IsValid)
            {
                _ServiceRepository.Add(service);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(service);
            }
        }

        // GET: ServiceController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServiceController/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            var editService = _ServiceRepository.GetById(service.ServiceID);
            if (editService == null) return NotFound();
            if(ModelState.IsValid)
            {
                _ServiceRepository.Update(editService);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(service);
            }
        }

        // GET: ServiceController/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: ServiceController/Delete/5
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
    }
}
