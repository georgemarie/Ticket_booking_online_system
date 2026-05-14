using BLL.Repository.implementaion;
using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Flight")]
    //[Authorize]
    public class FlightController : Controller
    {
        private readonly IFlightServiceRepository _flightRepo;
        public FlightController(IFlightServiceRepository flightRepo)
        {
            _flightRepo = flightRepo;
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
            return View();
        }

        // POST: Flights/Create
        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public ActionResult Create(FlightService flight)
        {
            if (ModelState.IsValid)
            {
                _flightRepo.Add(flight);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(flight);
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
        [HttpPost]
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
        [HttpPost]
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
