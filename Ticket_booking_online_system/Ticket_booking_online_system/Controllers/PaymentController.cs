using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Ticket_booking_online_system.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IGenericRepository<Payment> _paymentRepository;    

        public PaymentController(IBookingRepository bookingRepository, IGenericRepository<Payment> paymentRepository)
        {
            _bookingRepository = bookingRepository;
            _paymentRepository = paymentRepository; 
        }
        [HttpPost]
        public IActionResult CreateCheckoutSession(int bookingId)
        {
            var options = new SessionCreateOptions
            {
                Mode = "payment",
                SuccessUrl = $"http://localhost:5237/Payment/Success?bookingId={bookingId}",
                CancelUrl = $"http://localhost:5237/Payment/Cancel?bookingId={bookingId}",
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = 5000,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Hotel Booking"
                        }
                    },
                    Quantity = 1
                }
            }
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Json(new { id = session.Id });
        }


        public IActionResult Success(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);

            if (booking == null)
                return NotFound();

            // 1. Confirm booking
            booking.Status = BookingStatus.Confirmed;
            _bookingRepository.Update(booking);
            _bookingRepository.Save();

            // 2. Create payment record
            var payment = new Payment
            {
                Amount = 5000,
                Method = "CreditCard",
                Payment_Date = DateTime.Now,
                UserID = booking.UserID,
                BookingID = booking.BookingID
            };

            _paymentRepository.Add(payment);
            _paymentRepository.Save();

            return RedirectToAction("PaymentResult", new { id = bookingId });
        }

        [HttpGet("PaymentResult/{id:int}")]
        public IActionResult PaymentResult(int id)
        {
            var booking = _bookingRepository.GetBookingWithDetails(id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }
    }
}
   
