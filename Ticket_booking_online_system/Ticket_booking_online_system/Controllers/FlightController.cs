using BLL.Repository.implementaion;
using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Ticket_booking_online_system.Controllers
{
    [Authorize(Roles = "Admin")] // Entire controller is Admin Only
    [Route("Flight")]
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

        [HttpGet("")]
        public ActionResult Index()
        {
            var availableFlights = _flightRepo.GetAllWithIncludes();
            return View(availableFlights);
        }

        [HttpGet("Details/{id}")]
        [AllowAnonymous] // Or your preferred role
        public ActionResult Details(int id)
        {
            if (id < 0) return BadRequest();

            // FIX: Change GetById to GetByIdWithIncludes
            var flight = _flightRepo.GetByIdWithIncludes(id);

            if (flight == null) return NotFound();
            return View(flight);
        }

        [HttpGet("Create")]
        public ActionResult Create()
        {
            var airlines = _airlineRepo.GetAll();
            var locations = _locRepo.GetAll();

            ViewBag.Airlines = new SelectList(airlines, "Airline_ID", "Airline_Name");
            ViewBag.Locations = new SelectList(locations, "LocationID", "City");

            var model = new FlightService { Flight = new Flight(), Service = new Service() };
            return View(model);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FlightService model)
        {
            model.Flight ??= new Flight();
            model.Service ??= new Service();

            model.Flight_Number = model.Flight.Flight_Number;
            model.Service.ServiceType = "Flight";
            model.Service.LocationID = model.Flight.Origin_LocationID;

            ModelState.Remove(nameof(FlightService.Flight_Number));
            ModelState.Remove("Service.ServiceType");
            ModelState.Remove("Service.LocationID");
            ModelState.Remove("Flight.Airline");
            ModelState.Remove("Flight.OriginLocation");
            ModelState.Remove("Flight.DestLocation");
            ModelState.Remove("Flight.Bookings");
            ModelState.Remove("Service.Location");
            ModelState.Remove("Service.Reviews");
            ModelState.Remove("Service.Bookings");

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

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();

            var model = _flightRepo.GetByIdWithIncludes(id);
            if (model == null) return NotFound();

            model.Flight ??= new Flight();
            model.Service ??= new Service();

            ViewBag.Airlines = new SelectList(_airlineRepo.GetAll(), "Airline_ID", "Airline_Name", model.Flight.Airline_ID);
            ViewBag.Locations = new SelectList(_locRepo.GetAll(), "LocationID", "City");

            return View(model);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FlightService model)
        {
            if (id != model.Id) return BadRequest();

            var entity = _flightRepo.GetByIdWithIncludes(id);
            if (entity == null) return NotFound();

            entity.Flight ??= new Flight();
            entity.Service ??= new Service();
            ModelState.Remove("Flight.Airline");
            ModelState.Remove("Flight.OriginLocation");
            ModelState.Remove("Flight.DestLocation");
            ModelState.Remove("Service.Location");
            model.Flight_Number = model.Flight?.Flight_Number;
            ModelState.Remove(nameof(FlightService.Flight_Number));

            if (!ModelState.IsValid)
            {
                ViewBag.Airlines = new SelectList(_airlineRepo.GetAll(), "Airline_ID", "Airline_Name", model.Flight?.Airline_ID);
                ViewBag.Locations = new SelectList(_locRepo.GetAll(), "LocationID", "City");
                return View(model);
            }
            entity.Service.BasePrice = model.Service.BasePrice;
            entity.Service.ServiceType = "Flight";
            entity.Service.LocationID = model.Flight.Origin_LocationID;
            entity.Flight.Origin_LocationID = model.Flight.Origin_LocationID;
            entity.Flight.Dest_LocationID = model.Flight.Dest_LocationID;
            entity.Flight.Depart_Date = model.Flight.Depart_Date;
            entity.Flight.Arrival_Time = model.Flight.Arrival_Time;
            entity.Flight.Airline_ID = model.Flight.Airline_ID;
            entity.Flight.Available_Seats = model.Flight.Available_Seats;
            entity.Flight.Class = model.Flight.Class;

            _flightRepo.Update(entity);
            _flightRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();
            var model = _flightRepo.GetByIdWithIncludes(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(FlightService flight)
        {
            var deletedFlight = _flightRepo.GetById(flight.Id);
            if (deletedFlight == null) return NotFound();

            _flightRepo.Delete(deletedFlight);
            _flightRepo.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}