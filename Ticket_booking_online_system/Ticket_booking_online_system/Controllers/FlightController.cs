using BLL.Repository.implementaion;
using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Flight")]
    //[Authorize]
    public class FlightController : Controller
    {
        private readonly IFlightServiceRepository _flightRepo;
        private readonly IAirlineRepository _airlineRepo;
        private readonly ILocationRepository _locRepo;
        public FlightController(IFlightServiceRepository flightRepo, IAirlineRepository airlineRepo, ILocationRepository locRepo)
        {
            _flightRepo = flightRepo;
            _airlineRepo = airlineRepo;
            _locRepo = locRepo;
        }

        // GET: Flights
        //[AllowAnonymous]
        [HttpGet("")]
        public ActionResult Index()
        {
            var availableFlights = _flightRepo.GetAllWithIncludes();
            return View(availableFlights);
        }

        // GET: Flights/Details/5
        //[AllowAnonymous]
        [HttpGet("Details/{id}")]
        public ActionResult Details(int id)
        {
            if (id < 0) return BadRequest();
            var flight = _flightRepo.GetById(id);
            if (flight == null) return NotFound();
            return View(flight);
        }

        #region Admin Controllers
        // GET: Flights/Create
        //[Authorize(Roles = "Admin")]
        [HttpGet("Create")]
        public ActionResult Create()
        {
            var airlines = _airlineRepo.GetAll();
            // NEW: Load real locations from your repository
            // Replace '_locationRepo' with whatever your Location repository is named
            var locations = _locRepo.GetAll();

            ViewBag.Airlines = new SelectList(airlines, "Airline_ID", "Airline_Name");
            ViewBag.Locations = new SelectList(locations, "LocationID", "City"); // ID and City name

            var model = new FlightService { Flight = new Flight(), Service = new Service() };
            return View(model);
        }

        // POST: Flights/Create
        //[Authorize(Roles = "Admin")]
        //[HttpPost("Create")]
        //public ActionResult Create(FlightService flight)
        //{
        //    if (flight.Flight == null)
        //    {
        //        flight.Flight = new Flight
        //        {
        //            Flight_Number = flight.Flight_Number,
        //            // These won't be set here — they come from the form via Flight.xxx binding
        //            // So instead, just ensure the binder works by checking below
        //        };
        //    }

        //    Console.WriteLine($"After fix — Flight null: {flight.Flight == null}");
        //    Console.WriteLine($"Flight_Number: {flight.Flight?.Flight_Number}");

        //    ModelState.Remove("Flight");
        //    ModelState.Remove("Service");
        //    ModelState.Remove("Flight.Airline");
        //    ModelState.Remove("Flight.Airline_ID");
        //    ModelState.Remove("Flight.OriginLocation");
        //    ModelState.Remove("Flight.DestLocation");
        //    ModelState.Remove("Service.Location");

        //    if (ModelState.IsValid)
        //    {
        //        _flightRepo.Add(flight);   // saves Service + Flight inside
        //        _flightRepo.Save();        // saves FlightService
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewBag.Airlines = new SelectList(_airlineRepo.GetAll(), "Airline_ID", "Airline_Name");
        //    return View(flight);
        //}
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FlightService model)
        {
            model.Flight ??= new Flight();
            model.Service ??= new Service();

            // ✅ assign derived fields BEFORE validation
            model.Flight_Number = model.Flight.Flight_Number;
            model.Service.ServiceType = "Flight";
            model.Service.LocationID = model.Flight.Origin_LocationID;

            // ✅ clear ModelState for fields we set manually
            ModelState.Remove(nameof(FlightService.Flight_Number));
            ModelState.Remove("Service.ServiceType");
            ModelState.Remove("Service.LocationID");

            // ✅ remove validation for navigation props we don't bind
            ModelState.Remove("Flight.Airline");
            ModelState.Remove("Flight.OriginLocation");
            ModelState.Remove("Flight.DestLocation");
            ModelState.Remove("Flight.Bookings");

            ModelState.Remove("Service.Location");
            ModelState.Remove("Service.Reviews");
            ModelState.Remove("Service.Bookings");

            // ✅ validate nested objects (since you used ValidateNever earlier)
            TryValidateModel(model.Flight, "Flight");
            TryValidateModel(model.Service, "Service");

            if (!ModelState.IsValid)
            {
                ViewBag.Airlines = new SelectList(_airlineRepo.GetAll(), "Airline_ID", "Airline_Name", model.Flight.Airline_ID);
                ViewBag.Locations = new SelectList(_locRepo.GetAll(), "LocationID", "City");
                return View(model);
            }

            try
            {
                _flightRepo.Add(model);
                _flightRepo.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Database Error: " + (ex.InnerException?.Message ?? ex.Message));
                ViewBag.Airlines = new SelectList(_airlineRepo.GetAll(), "Airline_ID", "Airline_Name", model.Flight.Airline_ID);
                ViewBag.Locations = new SelectList(_locRepo.GetAll(), "LocationID", "City");
                return View(model);
            }
        }

        // GET: Flights/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            if (id < 0) return BadRequest();
            var flight = _flightRepo.GetById(id);
            if (flight == null) return NotFound();
            return View(flight);
        }

        // POST: Flights/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FlightService flight)
        {
            var editFlight = _flightRepo.GetById(flight.Id);
            if (editFlight == null) return NotFound();
            if (ModelState.IsValid)
            {
                _flightRepo.Update(editFlight);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(editFlight);
            }
        }

        // GET: Flights/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var flight = _flightRepo.GetById(id);
            if (flight == null) return NotFound();
            return View(flight);
        }

        // POST: Flights/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FlightService flight)
        {
            var deletedFlight = _flightRepo.GetById(flight.Id);
            if (deletedFlight == null) return NotFound();
            if (ModelState.IsValid)
            {
                _flightRepo.Update(deletedFlight);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(deletedFlight);
            }
        } 
        #endregion
    }
}
