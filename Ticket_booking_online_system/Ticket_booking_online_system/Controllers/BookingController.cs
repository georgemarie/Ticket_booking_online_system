using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Checkout;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

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
        #region Create Booking (Merged Flow)

        [Authorize(Roles = "User")]
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int serviceId)
        {
            // 1. Identify the logged-in user
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var service = _serviceRepository.GetById(serviceId);

            if (service == null || user == null)
                return NotFound("Service or User not found.");

            // 2. Pass useful info to the view for the "Confirmation" UI
            ViewBag.UserName = user.Name;
            ViewBag.UserPassport = user.Passport_num;
            ViewBag.UserEmail = user.Email;
            ViewBag.ServiceType = service.ServiceType;
            ViewBag.BasePrice = service.BasePrice;

            // 3. Prepare the booking model for the hidden fields in the form
            var booking = new Booking
            {
                ServiceID = serviceId,
                Date = DateTime.Now,
                Status = BookingStatus.Pending // Status remains pending until payment success
            };

            return View(booking);
        }

        [Authorize(Roles = "User")]
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking model)
        {
            // Remove navigation properties from validation as they aren't provided by the form
            ModelState.Remove("UserID");
            ModelState.Remove("User");
            ModelState.Remove("Service");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Service");
            }

            // 1. Get the current User ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 2. Create the booking record in the database first
            // Note: Use CreateBookingwithPayement to handle initial state/payment logic
            var bookingId = _bookingService.CreateBookingwithPayement(model.ServiceID, userId);
            var serviceEntity = _serviceRepository.GetById(model.ServiceID);

            // 3. Configure Stripe Checkout Session
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                // These URLs should point to your Success/Cancel actions in PaymentController
                SuccessUrl = $"http://localhost:5237/Payment/Success?bookingId={bookingId}",
                CancelUrl = $"http://localhost:5237/Payment/Cancel?bookingId={bookingId}",
                LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(serviceEntity.BasePrice * 100), // Stripe expects cents
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"SkyWings: {serviceEntity.ServiceType} Booking",
                        Description = $"Booking Ref: #{bookingId}"
                    }
                },
                Quantity = 1
            }
        }
            };

            // 4. Create Stripe Session and Redirect
            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

        #endregion

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