using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class BookingRepository
     : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context)
            : base(context)
        {
        }
        public IEnumerable<Booking> GetUserBookings(int userId)
        {
            return _context.Bookings
                .Include(b => b.Service)
                .Where(b => b.UserID == userId)
                .ToList();
        }

       
        public Booking GetBookingWithDetails(int bookingId)
        {
            return _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.User)
                .FirstOrDefault(b => b.BookingID == bookingId);
        }

       
        public void Cancel(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);

            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
                _context.Bookings.Update(booking);
            }
        }
    }
}
