using BLL.Repository.implementaion;
using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket_booking_online_system.Controllers
{
    [Route("Booking")]

    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IServiceRepository _serviceRepository;

        public BookingController(IBookingService bookingService, IBookingRepository bookingRepository, IGenericRepository<User> userRepository, IServiceRepository serviceRepository)
        {
            _bookingService = bookingService;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
        }
        #region Get All 
        // [Authorization(Roles = "Admin")]

        #region ALL BOOKINGS
        [HttpGet("")]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var bookings = _bookingRepository.GetAll();
            return View(bookings);
        }
        #endregion
        #endregion
        #region UserBookings

        // GET: /Booking/UserBookings/5
        [HttpGet("UserBookings/{userId:int}")]
        public IActionResult UserBookings(int userId)
        {
            var bookings = _bookingRepository.GetUserBookings(userId);
            return View(bookings);
        }
        #endregion
        #region Details Of Booking

        // GET: /Booking/Details/5
        [HttpGet("Details/{id:int}")]
        public IActionResult Details(int id)
        {
            var booking = _bookingRepository.GetBookingWithDetails(id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        #endregion

        #region Create Booking

        // GET: /Booking/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.UserID = new SelectList(_userRepository.GetAll(), "UserID", "Name");
            ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType");
            return View();
        }

        // POST: /Booking/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Booking model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(_userRepository.GetAll(), "UserID", "Name");
                ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType");
                return View(model);
            }
            //MUST BE LOGGINED 
            // int userId = model.UserID;

            _bookingService.CreateBooking(model.ServiceID, model.UserID);
            return RedirectToAction(nameof(UserBookings), new { userId = model.UserID });
        }

        #endregion

        #region Edit booking 
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var booking = _bookingRepository.GetById(id);

            if (booking == null)
                return NotFound();
            ViewBag.UserID = new SelectList(_userRepository.GetAll(), "UserID", "Name", booking.UserID);
            ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType", booking.ServiceID);

            return View(booking);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Booking model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserID = new SelectList(_userRepository.GetAll(), "UserID", "Name", model.UserID);
                ViewBag.ServiceID = new SelectList(_serviceRepository.GetAll(), "ServiceID", "ServiceType", model.ServiceID);
                return View(model);
            }

            var booking = _bookingRepository.GetById(model.BookingID);

            if (booking == null)
                return NotFound();


            booking.Date = model.Date;
            booking.ServiceID = model.ServiceID;

            _bookingRepository.Update(booking);
            _bookingRepository.Save();

            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Cancel Booking
        // POST: /Booking/Cancel/5
        [HttpPost("Cancel/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var result = _bookingService.CancelBooking(id);

            if (!result)
                return BadRequest("Cannot cancel this booking");

            return RedirectToAction(nameof(Index));
        }


        #endregion

    }
}