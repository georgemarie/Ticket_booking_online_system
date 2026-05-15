using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ticket_booking_online_system.Controllers
{
    [Authorize] // Everyone interacting with bookings must be logged in
    [Route("Booking")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(
            IBookingService bookingService,
            IBookingRepository bookingRepository,
            IServiceRepository serviceRepository,
            UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _bookingRepository = bookingRepository;
            _serviceRepository = serviceRepository;
            _userManager = userManager;
        }

        // --- ADMIN: ALL BOOKINGS DASHBOARD ---
        [Authorize(Roles = "Admin")]
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var bookings = _bookingRepository.GetAll();
            return View(bookings);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var booking = _bookingRepository.GetById(id);
            if (booking == null) return NotFound();

            ViewBag.UserID = new SelectList(_userManager.Users.ToList(), "Id", "Name", booking.UserID);
            ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType", booking.ServiceID);

            return View(booking);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Booking model)
        {
            ModelState.Remove("User");
            ModelState.Remove("Service");

            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(_userManager.Users.ToList(), "Id", "Name", model.UserID);
                ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType", model.ServiceID);
                return View(model);
            }

            var booking = _bookingRepository.GetById(model.BookingID);
            if (booking == null) return NotFound();

            booking.Date = model.Date;
            booking.ServiceID = model.ServiceID;
            booking.UserID = model.UserID;

            _bookingRepository.Update(booking);
            _bookingRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        // --- USER: BOOKING FLOW ---
        [Authorize(Roles = "User")]
        [HttpGet("UserBookings/{userId}")]
        public IActionResult UserBookings(string userId)
        {
            var bookings = _bookingRepository.GetUserBookings(userId);
            return View(bookings);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int serviceId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var service = _serviceRepository.GetById(serviceId);

            if (service == null || user == null) return NotFound("Service or User not found.");

            ViewBag.UserName = user.Name;
            ViewBag.UserPassport = user.Passport_num;
            ViewBag.UserEmail = user.Email;
            ViewBag.ServiceType = service.ServiceType;
            ViewBag.BasePrice = service.BasePrice;

            var booking = new Booking
            {
                ServiceID = serviceId,
                Date = DateTime.Now,
                Status = BookingStatus.Pending
            };

            return View(booking);
        }

        [Authorize(Roles = "User")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking model)
        {
            ModelState.Remove("UserID");
            ModelState.Remove("User");
            ModelState.Remove("Service");

            if (!ModelState.IsValid) return RedirectToAction("Index", "Service");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _bookingService.CreateBooking(model.ServiceID, userId);

            return RedirectToAction(nameof(UserBookings), new { userId = userId });
        }

        // --- SHARED CAPABILITIES ---
        [Authorize(Roles = "User,Admin")]
        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var booking = _bookingRepository.GetBookingWithDetails(id);
            if (booking == null) return NotFound();
            return View(booking);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost("Cancel/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var result = _bookingService.CancelBooking(id);
            if (!result) return BadRequest("Cannot cancel this booking");

            if (User.IsInRole("Admin")) return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(UserBookings), new { userId = User.FindFirstValue(ClaimTypes.NameIdentifier) });
        }
    }
}